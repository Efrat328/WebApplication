using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = Repository.Entities.TaskStatus;



using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Dto;
using Service.Interface;

namespace Service.Services
{
    public class TaskService : IService<TaskItemDto>
    {
        private readonly IRepository<TaskItem> _repository;
        private readonly IMapper _mapper;

        public TaskService(IRepository<TaskItem> repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }
        public async Task<TaskItemDto> AddItem(TaskItemDto item)
        {
            if (item == null) throw new ArgumentNullException("item");
            List<TaskItemDto> taskItems = new List<TaskItemDto>();
            taskItems = await GetAll();
            foreach (TaskItemDto taskItem in taskItems)
            {
                if (taskItem.Title == item.Title)
                    throw new Exception("The Title is already exists");
            }
            item.Status = TaskStatus.Open;
            item.AssignedTo = null;
            if (item.Deadline < DateTime.Now)
                throw new Exception("The deadline must be in the future");
            if ((item.Deadline - DateTime.Now).Days < item.Expected)
            {
                item.Priority = TaskPriorityDto.High;
                //await SplitIfHighPriority(item.Id);        
            }


            else if ((item.Deadline - DateTime.Now).Days < item.Expected * 2)
                item.Priority = TaskPriorityDto.Low;
            else item.Priority = TaskPriorityDto.Medium;
            item.StartedAt = DateTime.Now;
            var savedTask = await _repository.AddItem(_mapper.Map<TaskItem>(item));

            // אם High ויש תתי משימות — מפצל
            if (item.Priority == TaskPriorityDto.High)
                await SplitIfHighPriority(savedTask.Id);

            return _mapper.Map<TaskItemDto>(savedTask);
        }
        public async Task DeleteItem(int id)
        {
            TaskItem taskItem = await _repository.GetById(id);
            if (taskItem == null) throw new ArgumentNullException(nameof(id));
            foreach (var subTask in taskItem.SubTasks)
            {
                subTask.Status = SubTaskStatus.Canceled;
            }
            taskItem.Status = TaskStatus.Canceled;
            await _repository.UpdateItem(taskItem);


        }
        public async Task<List<TaskItemDto>> GetAll()
        {
            return _mapper.Map<List<TaskItemDto>>(await _repository.GetAll());
        }
        public async Task<TaskItemDto> GetById(int id)
        {
            return _mapper.Map<TaskItemDto>(await _repository.GetById(id));
        }
        public async Task UpdateItem(int id, TaskItemDto item)
        {
            TaskItem taskItem = await _repository.GetById(id);
            if (taskItem == null)
            {
                throw new ArgumentNullException(nameof(id));

            }
            taskItem.Description = item.Description;
            taskItem.AssignedTo = item.AssignedTo;  // int from-DTO
            taskItem.Title = item.Title;
            taskItem.Status = item.Status;

            taskItem.Deadline = item.Deadline;
            if (taskItem.Status == TaskStatus.Completed)
            {
                taskItem.CompletedAt = DateTime.Now;

            }
            await UpdatePriority(id, item);
            await _repository.UpdateItem(taskItem);
            if (item.Priority == TaskPriorityDto.High)
                await SplitIfHighPriority(id);

        }
        //public void UpdateStatus(int id, TaskStatus status)
        public async Task UpdatePriority(int id, TaskItemDto item)
        {
            TaskItem taskItem = await _repository.GetById(id);
            List<SubTask> subTasks = taskItem.SubTasks.ToList();
            int count = 0, completeSubTask = 0, score = 0, days;
            foreach (var subTask in subTasks)
            {
                if (subTask.Status == SubTaskStatus.Completed)
                    count++;
            }
            if (count == 0) return;
            completeSubTask = (subTasks.Count / count) * 100;
            days = (taskItem.Deadline - DateTime.Now).Days;
            score = days / (taskItem.Expected * (1 - completeSubTask));
            if (score < 1)
                item.Priority = TaskPriorityDto.High;
            else if (score <= 2)
                item.Priority = TaskPriorityDto.Medium;
            else
                item.Priority = TaskPriorityDto.Low;
        }
        public async Task SplitIfHighPriority(int id)
        {
            TaskItem original = await _repository.GetById(id);
            if (original == null) throw new ArgumentNullException(nameof(id));

            var allTasks = await _repository.GetAll();
    
            // מצא את המשימה האחרונה בשרשרת שעדיין לא פוצלה
            TaskItem target = original;
            while (true)
            {
                bool alreadySplit = allTasks.Any(t => t.Title == target.Title + " (המשך)");
                if (!alreadySplit) break; // מצאנו את הקצה
        
                // עבור למשימה הבאה בשרשרת
                target = allTasks.First(t => t.Title == target.Title + " (המשך)");
                target = await _repository.GetById(target.Id); // טען עם SubTasks
            }

            var pendingSubTasks = target.SubTasks
            .Where(st => st.Status == SubTaskStatus.Open)
            .ToList();

            if (!pendingSubTasks.Any()) return;

            var firstHalf  = pendingSubTasks.Take(pendingSubTasks.Count / 2).ToList();
            var secondHalf = pendingSubTasks.Skip(pendingSubTasks.Count / 2).ToList();

            var newTask1 = new TaskItem
            {
                ProjectId   = target.ProjectId,
                Title       = target.Title + " (המשך)",
                Description = target.Description,
                Expected    = (target.Expected / target.SubTasks.Count) * firstHalf.Count,
                Status      = TaskStatus.Open,
                AssignedTo  = null,
                StartedAt   = DateTime.Now,
                Deadline    = target.Deadline,
                SubTasks    = firstHalf
            };
            var newTask2 = new TaskItem
            {
                ProjectId   = target.ProjectId,
                Title       = target.Title + " (המשך נוסף)",
                Description = target.Description,
                Expected    = (target.Expected / target.SubTasks.Count) * secondHalf.Count,
                Status      = TaskStatus.Open,
                AssignedTo  = null,
                StartedAt   = DateTime.Now,
                Deadline    = target.Deadline,
                SubTasks    = secondHalf
            };

            foreach (var st in pendingSubTasks)
            st.Status = SubTaskStatus.Canceled;

            await _repository.AddItem(newTask1);
            await _repository.AddItem(newTask2);
            await _repository.UpdateItem(target);
        } 
        public async Task<TaskPriority> GetPriority(int id)
        {
            TaskItem task = await _repository.GetById(id);
            if (task == null) throw new ArgumentNullException(nameof(id));
            return task.Priority;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TaskItemDto> AddItem(TaskItemDto item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var taskItems = await GetAll();
            if (taskItems.Any(t => t.Title == item.Title))
                throw new Exception("The Title already exists");

            if (item.Deadline <= DateTime.Now)
                throw new Exception("The deadline must be in the future");

            item.Status    = TaskStatus.Open;
            item.AssignedTo = null;
            item.StartedAt  = DateTime.Now;

            // חישוב עדיפות לפי פער הזמן
            double daysLeft = (item.Deadline - DateTime.Now).TotalDays;
            if (daysLeft < item.Expected)
                item.Priority = TaskPriorityDto.High;
            else if (daysLeft < item.Expected * 2)
                item.Priority = TaskPriorityDto.Low;
            else
                item.Priority = TaskPriorityDto.Medium;

            var savedTask = await _repository.AddItem(_mapper.Map<TaskItem>(item));

            if (item.Priority == TaskPriorityDto.High)
                await SplitIfHighPriority(savedTask.Id);

            return _mapper.Map<TaskItemDto>(savedTask);
        }

        public async Task DeleteItem(int id)
        {
            var taskItem = await _repository.GetById(id);
            if (taskItem == null) throw new ArgumentNullException(nameof(id));

            foreach (var subTask in taskItem.SubTasks)
                subTask.Status = SubTaskStatus.Canceled;

            taskItem.Status = TaskStatus.Canceled;
            await _repository.UpdateItem(taskItem);
        }

        public async Task<List<TaskItemDto>> GetAll()
            => _mapper.Map<List<TaskItemDto>>(await _repository.GetAll());

        public async Task<TaskItemDto> GetById(int id)
            => _mapper.Map<TaskItemDto>(await _repository.GetById(id));

        public async Task UpdateItem(int id, TaskItemDto item)
        {
            var taskItem = await _repository.GetById(id);
            if (taskItem == null) throw new ArgumentNullException(nameof(id));

            taskItem.Description = item.Description;
            taskItem.AssignedTo  = item.AssignedTo;
            taskItem.Title       = item.Title;
            taskItem.Status      = item.Status;
            taskItem.Deadline    = item.Deadline;

            if (taskItem.Status == TaskStatus.Completed)
                taskItem.CompletedAt = DateTime.Now;

            await UpdatePriority(id, item);

            taskItem.Priority = _mapper.Map<TaskPriority>(item.Priority); // ✅ סנכרן את העדיפות
            await _repository.UpdateItem(taskItem);

            if (item.Priority == TaskPriorityDto.High)
                await SplitIfHighPriority(id);
        }

        public async Task UpdatePriority(int id, TaskItemDto item)
        {
            var taskItem = await _repository.GetById(id);
            var subTasks = taskItem.SubTasks.ToList();

            if (!subTasks.Any()) return;

            int completedCount = subTasks.Count(st => st.Status == SubTaskStatus.Completed);
            if (completedCount == 0) return;

            // ✅ תיקון: אחוז השלמה נכון (completed/total)
            double completedRatio = (double)completedCount / subTasks.Count;
            double remaining      = 1.0 - completedRatio;

            // אם הכל הושלם — עדיפות נמוכה
            if (remaining <= 0)
            {
                item.Priority = TaskPriorityDto.Low;
                return;
            }

            double daysLeft = (taskItem.Deadline - DateTime.Now).TotalDays;
            // ✅ תיקון: שימוש ב-double למניעת חלוקת int
            double score = daysLeft / (taskItem.Expected * remaining);

            if (score < 1)
                item.Priority = TaskPriorityDto.High;
            else if (score <= 2)
                item.Priority = TaskPriorityDto.Medium;
            else
                item.Priority = TaskPriorityDto.Low;
        }

        public async Task SplitIfHighPriority(int id)
        {
            var original = await _repository.GetById(id);
            if (original == null) throw new ArgumentNullException(nameof(id));

            var allTasks = await _repository.GetAll();

            // מצא את הקצה האחרון בשרשרת שעדיין לא פוצל
            TaskItem target = original;
            while (allTasks.Any(t => t.Title == target.Title + " (המשך)"))
            {
                var next = allTasks.First(t => t.Title == target.Title + " (המשך)");
                target = await _repository.GetById(next.Id);
            }

            var pendingSubTasks = target.SubTasks
                .Where(st => st.Status == SubTaskStatus.Open)
                .ToList();

            if (!pendingSubTasks.Any()) return;

            int half        = pendingSubTasks.Count / 2;
            var firstHalf   = pendingSubTasks.Take(half).ToList();
            var secondHalf  = pendingSubTasks.Skip(half).ToList();

            int totalSubs   = target.SubTasks.Count > 0 ? target.SubTasks.Count : 1; // ✅ מניעת חלוקה ב-0

            // ✅ תיקון מרכזי: צור SubTasks חדשים במקום להעביר את אותם objects
            var newTask1 = new TaskItem
            {
                ProjectId   = target.ProjectId,
                Title       = target.Title + " (המשך)",
                Description = target.Description,
                Expected    = (target.Expected / totalSubs) * firstHalf.Count,
                Status      = TaskStatus.Open,
                AssignedTo  = null,
                StartedAt   = DateTime.Now,
                Deadline    = target.Deadline,
                SubTasks    = firstHalf.Select(st => new SubTask
                {
                    Title       = st.Title,
                    Description = st.Description,
                    Status      = SubTaskStatus.Open
                }).ToList()
            };

            var newTask2 = new TaskItem
            {
                ProjectId   = target.ProjectId,
                Title       = target.Title + " (המשך נוסף)",
                Description = target.Description,
                Expected    = (target.Expected / totalSubs) * secondHalf.Count,
                Status      = TaskStatus.Open,
                AssignedTo  = null,
                StartedAt   = DateTime.Now,
                Deadline    = target.Deadline,
                SubTasks    = secondHalf.Select(st => new SubTask
                {
                    Title       = st.Title,
                    Description = st.Description,
                    Status      = SubTaskStatus.Open
                }).ToList()
            };

            // בטל את תתי-המשימות המקוריות
            foreach (var st in pendingSubTasks)
                st.Status = SubTaskStatus.Canceled;

            await _repository.UpdateItem(target);   // ✅ שמור ביטול לפני הוספת חדשים
            await _repository.AddItem(newTask1);
            await _repository.AddItem(newTask2);
        }

        public async Task<TaskPriority> GetPriority(int id)
        {
            var task = await _repository.GetById(id);
            if (task == null) throw new ArgumentNullException(nameof(id));
            return task.Priority;
        }
    }
}
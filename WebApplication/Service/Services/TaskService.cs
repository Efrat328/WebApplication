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

            item.Status = TaskStatus.Open;
            item.AssignedTo = null;
            item.StartedAt = DateTime.Now;

            double daysLeft = (item.Deadline - DateTime.Now).TotalDays;
            if (daysLeft < item.Expected)
                item.Priority = TaskPriorityDto.High;
            else if (daysLeft > item.Expected * 2)
                item.Priority = TaskPriorityDto.Low;
            else
                item.Priority = TaskPriorityDto.Medium;

            // אם המשימה High — לא מאפשר להוסיף תת-משימות
            if (item.Priority == TaskPriorityDto.High)
                throw new Exception("Cannot add tasks to a High Priority task. The daily job will handle splitting.");

            var savedTask = await _repository.AddItem(_mapper.Map<TaskItem>(item));

            return _mapper.Map<TaskItemDto>(savedTask);
        }

        public async Task DeleteItem(int id)
        {
            var taskItem = await _repository.GetById(id);
            if (taskItem == null)
                throw new ArgumentNullException(nameof(id));

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
            taskItem.AssignedTo = item.AssignedTo;
            taskItem.Title = item.Title;
            taskItem.Status = item.Status;
            taskItem.Deadline = item.Deadline;

            if (taskItem.Status == TaskStatus.Completed)
                taskItem.CompletedAt = DateTime.Now;

            await UpdatePriority(id, item);

            taskItem.Priority = _mapper.Map<TaskPriority>(item.Priority);
            await _repository.UpdateItem(taskItem);


        }

        public async Task UpdatePriority(int id, TaskItemDto item)
        {
            var taskItem = await _repository.GetById(id);
            var subTasks = taskItem.SubTasks.ToList();

            if (!subTasks.Any()) return;

            int completedCount = subTasks.Count(st => st.Status == SubTaskStatus.Completed);
            if (completedCount == 0) return;

            double completedRatio = (double)completedCount / subTasks.Count;
            double remaining = 1.0 - completedRatio;

            if (remaining <= 0)
            {
                item.Priority = TaskPriorityDto.Low;
                return;
            }

            double daysLeft = (taskItem.Deadline - DateTime.Now).TotalDays;
            double score = daysLeft / (taskItem.Expected * remaining);

            if (score < 1)
                item.Priority = TaskPriorityDto.High;
            else if (score <= 2)
                item.Priority = TaskPriorityDto.Medium;
            else
                item.Priority = TaskPriorityDto.Low;
            if (item.Priority == TaskPriorityDto.High)
                await SplitIfHighPriority(id);
        }

        public async Task SplitIfHighPriority(int id)
        {
            var task = await _repository.GetById(id);
            if (task == null) throw new ArgumentNullException(nameof(id));

            if (task.Priority != TaskPriority.High)
                return;

            var allTasks = await _repository.GetAll();

            var continuation = allTasks
                .FirstOrDefault(t => t.Title == task.Title + " (המשך)");

            // 🟢 יש המשך
            if (continuation != null)
            {
                var target = await _repository.GetById(continuation.Id);

                var pending = task.SubTasks
                    .Where(st => st.Status == SubTaskStatus.Open)
                    .ToList();

                if (!pending.Any()) return; // 🔥 חשוב

                foreach (var st in pending)
                {
                    target.SubTasks.Add(new SubTask
                    {
                        Title = st.Title,
                        Description = st.Description,
                        Status = SubTaskStatus.Open
                    });

                    st.Status = SubTaskStatus.Canceled;
                }

                await _repository.UpdateItem(task);
                await _repository.UpdateItem(target);

                await SplitIfHighPriority(target.Id);
                return;
            }

            // 🔴 אין המשך → פיצול
            var pendingSubTasks = task.SubTasks
                .Where(st => st.Status == SubTaskStatus.Open)
                .ToList();

            if (pendingSubTasks.Count < 2) return;

            int half = pendingSubTasks.Count / 2;

            var firstHalf = pendingSubTasks.Take(half).ToList();
            var secondHalf = pendingSubTasks.Skip(half).ToList();

            var newTask1 = new TaskItem
            {
                ProjectId = task.ProjectId,
                Title = task.Title + " (המשך)",
                Description = task.Description,
                Status = TaskStatus.Open,
                Priority = TaskPriority.High, // 🔥 חשוב
                StartedAt = DateTime.Now,
                Deadline = task.Deadline,
                SubTasks = firstHalf.Select(st => new SubTask
                {
                    Title = st.Title,
                    Description = st.Description,
                    Status = SubTaskStatus.Open
                }).ToList()
            };

            var newTask2 = new TaskItem
            {
                ProjectId = task.ProjectId,
                Title = task.Title + " (המשך נוסף)",
                Description = task.Description,
                Status = TaskStatus.Open,
                Priority = TaskPriority.High, // 🔥 חשוב
                StartedAt = DateTime.Now,
                Deadline = task.Deadline,
                SubTasks = secondHalf.Select(st => new SubTask
                {
                    Title = st.Title,
                    Description = st.Description,
                    Status = SubTaskStatus.Open
                }).ToList()
            };

            foreach (var st in pendingSubTasks)
                st.Status = SubTaskStatus.Canceled;

            await _repository.UpdateItem(task);
            await _repository.AddItem(newTask1);
            await _repository.AddItem(newTask2);

            await SplitIfHighPriority(newTask1.Id);
            await SplitIfHighPriority(newTask2.Id);
        }
    }
}
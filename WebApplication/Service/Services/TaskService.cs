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
        public TaskItemDto AddItem(TaskItemDto item)
        {
            if (item == null) throw new ArgumentNullException("item");
            List<TaskItemDto> taskItems = new List<TaskItemDto>();
            taskItems = GetAll();
            foreach (TaskItemDto taskItem in taskItems)
            {
                if (taskItem.Title == item.Title)
                    throw new Exception("The Title is already exists");
            }
            item.Status = TaskStatus.Open;
            if (item.Deadline < DateTime.Now)
                throw new Exception("The deadline must be in the future");
            if ((item.Deadline - DateTime.Now).Days < item.Expected)
                item.Priority = TaskPriorityDto.High;
            else if ((item.Deadline - DateTime.Now).Days < item.Expected * 2)
                item.Priority = TaskPriorityDto.Low;
            else item.Priority = TaskPriorityDto.Medium;
            item.StartedAt = DateTime.Now;
            return _mapper.Map<TaskItemDto>(_repository.AddItem(_mapper.Map<TaskItem>(item)));
        }
        public void DeleteItem(int id)
        {
            TaskItem taskItem = _repository.GetById(id);
            if (taskItem == null) throw new ArgumentNullException(nameof(id));
            foreach (var subTask in taskItem.SubTasks)
            {
                subTask.Status = SubTaskStatus.Canceled;
            }
            taskItem.Status = TaskStatus.Canceled;
            _repository.UpdateItem(taskItem);
        }
        public List<TaskItemDto> GetAll()
        {
            return _mapper.Map<List<TaskItemDto>>(_repository.GetAll());
        }
        public TaskItemDto GetById(int id)
        {
            return _mapper.Map<TaskItemDto>(_repository.GetById(id));
        }
        public void UpdateItem(int id, TaskItemDto item)
        {
            TaskItem taskItem = _repository.GetById(id);
            if (taskItem == null)
            {
                throw new ArgumentNullException(nameof(id));

            }
            taskItem.Description = item.Description;
            taskItem.AssignedTo = item.AssignedTo;  // int from-DTO
            taskItem.Title = item.Title;
            taskItem.Status = item.Status;
            //taskItem.Priority = item.Priority;
            taskItem.Deadline = item.Deadline;
            if (taskItem.Status == TaskStatus.Completed)
            {
                taskItem.CompletedAt = DateTime.Now;
                _repository.UpdateItem(taskItem);
            }

        }
        //public void UpdateStatus(int id, TaskStatus status)
        public void UpdatePriority(int id,  TaskItemDto item)
        {
            
            List<SubTask> subTasks = _repository.GetById(id).SubTasks.ToList();
            int count = 0,completeSubTask=0,score=0,days;
            foreach (var subTask in subTasks)
            {
                if (subTask.Status == SubTaskStatus.Completed)
                    count++;    
            }
            completeSubTask=(subTasks.Count/count)*100;
            days = (_repository.GetById(id).Deadline - DateTime.Now).Days;
            score =days/(_repository.GetById(id).Expected*(1-completeSubTask));
            if (score < 1)
                item.Priority = TaskPriorityDto.High;
            else if (score <= 2)
                item.Priority = TaskPriorityDto.Medium;
            else
                item.Priority = TaskPriorityDto.Low;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Repository.Repositories;
using Service.Dto;
using Service.Interface;

namespace Service.Services
{
    public class SubTaskService : IService<SubTaskDto>
    {
        private readonly IRepository<SubTask> _repository;
        private readonly HistoryService _historyService;
        private readonly IMapper _mapper;
        private readonly TaskService _taskService;

        public SubTaskService(IRepository<SubTask> repository, IMapper mapper, HistoryService historyService, TaskService taskService)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._historyService = historyService;
            this._taskService = taskService;
        }
        public async Task<SubTaskDto> AddItem(SubTaskDto item)
        {
            if (item == null) throw new ArgumentNullException("item");
            List<SubTaskDto> subTasks = await GetAll();
            foreach (SubTaskDto subTask in subTasks)
            {
                if (subTask.Title == item.Title)
                    throw new Exception("This subTask is already exists");
            }
            var priorTask = await _taskService.GetPriority(item.TaskId);
            //if(priorTask==TaskPriority.High)

            
                //throw new Exception("The TaskId is not exists");
            var saved = await _repository.AddItem(_mapper.Map<SubTask>(item));
    
            // אחר כך מפצל
            await _taskService.SplitIfHighPriority(item.TaskId);
    
            return _mapper.Map<SubTaskDto>(saved);
        }
        public async Task DeleteItem(int id)
        {
            SubTask subTask = await _repository.GetById(id);
            if (subTask == null) throw new ArgumentNullException(nameof(id));
            subTask.Status = SubTaskStatus.Canceled;
            await _repository.UpdateItem(subTask);

        }
        public async Task<List<SubTaskDto>> GetAll()
        {
            return _mapper.Map<List<SubTaskDto>>(await _repository.GetAll());
        }   
        public async Task<SubTaskDto> GetById(int id)
        {
            return _mapper.Map<SubTaskDto>(await _repository.GetById(id));
        }
        public async Task UpdateItem(int id, SubTaskDto item)
        {
            SubTask subTask = await _repository.GetById(id);
            if (subTask != null)
            {
                await _historyService.AddHistory(subTask.Status, item.Status, id);
                subTask.Title = item.Title;
                subTask.Description = item.Description;
                subTask.AssignedTo = item.AssignedTo;
                //subTask.Deadline = item.Deadline;
                subTask.Status = item.Status;
                await _repository.UpdateItem(_mapper.Map<SubTask>(subTask));
            }
            else
            {
                throw new ArgumentNullException(nameof(id));
            }
}
    }
}
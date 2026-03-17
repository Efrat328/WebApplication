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

        public SubTaskService(IRepository<SubTask> repository, IMapper mapper, HistoryService historyService)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._historyService = historyService;
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
            return _mapper.Map<SubTaskDto>(await _repository.AddItem(_mapper.Map<SubTask>(item)));
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
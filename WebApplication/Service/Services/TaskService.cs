using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return _mapper.Map<TaskItemDto>(_repository.AddItem(_mapper.Map<TaskItem>(item)));
        }
        public void DeleteItem(int id)
        {
            _repository.DeleteItem(id);
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
            _repository.UpdateItem(id, _mapper.Map<TaskItem>(item));
        }
    }
}

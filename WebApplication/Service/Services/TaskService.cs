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
    public class TaskService : IService<TaskDto>
    {
        private readonly IRepository<TaskItem> _repository;
        private readonly IMapper _mapper;

        public TaskService(IRepository<TaskItem> repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }
        public TaskDto AddItem(TaskDto item)
        {
            return _mapper.Map<TaskDto>(_repository.AddItem(_mapper.Map<TaskItem>(item)));
        }
        public void DeleteItem(int id)
        {
            _repository.DeleteItem(id);
        }
        public List<TaskDto> GetAll()
        {
            return _mapper.Map<List<TaskDto>>(_repository.GetAll());
        }
        public TaskDto GetById(int id)
        {
            return _mapper.Map<TaskDto>(_repository.GetById(id));
        }
        public void UpdateItem(int id, TaskDto item)
        {
            _repository.UpdateItem(id, _mapper.Map<TaskItem>(item));
        }
    }
}

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
    public class SubTaskService : IService<SubTaskDto>
    {
        private readonly IRepository<SubTask> _repository;
        private readonly IMapper _mapper;

        public SubTaskService(IRepository<SubTask> repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }
        public SubTaskDto AddItem(SubTaskDto item)
        {
            return _mapper.Map<SubTaskDto>(_repository.AddItem(_mapper.Map<SubTask>(item)));
        }
        public void DeleteItem(int id)
        {
            _repository.DeleteItem(id);
        }
        public List<SubTaskDto> GetAll()
        {
            return _mapper.Map<List<SubTaskDto>>(_repository.GetAll());
        }
        public SubTaskDto GetById(int id)
        {
            return _mapper.Map<SubTaskDto>(_repository.GetById(id));
        }
        public void UpdateItem(int id, SubTaskDto item)
        {
            _repository.UpdateItem(id, _mapper.Map<SubTask>(item));
        }
    }
}
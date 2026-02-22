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
    public class ProjectService : IService<ProjectDto>
    {
        private readonly IRepository<Project> _repository;
        private readonly IMapper _mapper;

        public ProjectService(IRepository<Project> repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }
        public ProjectDto AddItem(ProjectDto item)
        {
            return _mapper.Map<ProjectDto>(_repository.AddItem(_mapper.Map<Project>(item)));
        }
        public void DeleteItem(int id)
        {
            _repository.DeleteItem(id);
        }
        public List<ProjectDto> GetAll()
        {
            return _mapper.Map<List<ProjectDto>>(_repository.GetAll());
        }
        public ProjectDto GetById(int id)
        {
            return _mapper.Map<ProjectDto>(_repository.GetById(id));
        }
        public void UpdateItem(int id, ProjectDto item)
        {
            _repository.UpdateItem(id, _mapper.Map<Project>(item));
        }
    }
}
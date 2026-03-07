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
            if (item == null) throw new ArgumentNullException("item");
            List<ProjectDto> projects = new List<ProjectDto>();
            projects = GetAll();
            foreach (ProjectDto project in projects)
            {
                if (project.NameProject == item.NameProject)
                    throw new Exception("This project is already exists");
            }
            return _mapper.Map<ProjectDto>(_repository.AddItem(_mapper.Map<Project>(item)));
        }
        public void DeleteItem(int id)
        {
            Project project = _repository.GetById(id);
            if (project == null) throw new ArgumentNullException(nameof(id));
            project.Status = Canceled;
            _repository.UpdateItem(project);
           
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
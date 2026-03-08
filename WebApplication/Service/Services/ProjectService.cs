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
using TaskStatus = Repository.Entities.TaskStatus;


namespace Service.Services
{
    public class ProjectService : IService<ProjectDto>
    {
        private readonly IRepository<Project> _repository;
        private readonly TaskService _taskService;
        private readonly IMapper _mapper;

        public ProjectService(IRepository<Project> repository, IMapper mapper, TaskService taskService)
        {
            this._repository = repository;
            this._mapper = mapper;
            _taskService = taskService;
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
            project.Status = ProjectStatus.Canceled;
            foreach (TaskItem task in project.Tasks)
            {           
                 _taskService.DeleteItem(task.Id);
            }
            _repository.UpdateItem(project);      
        }
        public List<ProjectDto> GetAll()
        {
            return _mapper.Map<List<ProjectDto>>(_repository.GetAll());
        }
        public ProjectDto GetById(int id)
        {
            if(id==null) throw new ArgumentNullException(nameof(id));
            return _mapper.Map<ProjectDto>(_repository.GetById(id));
        }
        public void UpdateItem(int id, ProjectDto item)
        {
            Project project = _repository.GetById(id);
            if (project != null)
            {
                project.NameProject = item.NameProject;
                project.Description = item.Description;
                project.Deadline = item.Deadline;
                project.Status = item.Status;            
            }
            else
            {
                throw new ArgumentNullException(nameof(id));
            }
            _repository.UpdateItem( _mapper.Map<Project>(project));
        }
    }
}
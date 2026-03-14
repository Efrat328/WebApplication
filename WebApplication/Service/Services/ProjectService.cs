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
        private readonly IService<TaskItemDto> _taskService;
        private readonly IMapper _mapper;

        public ProjectService(IRepository<Project> repository, IMapper mapper, IService<TaskItemDto> taskService)
        {
            this._repository = repository;
            this._mapper = mapper;
            _taskService = taskService;
        }
        public async Task<ProjectDto> AddItem(ProjectDto item)
        {
            if (item == null) throw new ArgumentNullException("item");
            List<ProjectDto> projects = new List<ProjectDto>();
            projects = await GetAll();
            foreach (ProjectDto project in projects)
            {
                if (project.NameProject == item.NameProject)
                    throw new Exception("This project is already exists");
            }
            return _mapper.Map<ProjectDto>(await _repository.AddItem(_mapper.Map<Project>(item)));
        }
        public async Task DeleteItem(int id)
        {
            Project project = await _repository.GetById(id);
            if (project == null) throw new ArgumentNullException(nameof(id));
            project.Status = ProjectStatus.Canceled;
            foreach (TaskItem task in project.Tasks)
            {           
                 await _taskService.DeleteItem(task.Id);
            }
             await _repository.UpdateItem(project);      
        }
        public async Task<List<ProjectDto>> GetAll()
        {
            return _mapper.Map<List<ProjectDto>>(await _repository.GetAll());
        }
        public async Task<ProjectDto> GetById(int id)
        {
            
            return _mapper.Map<ProjectDto>(await _repository.GetById(id));
        }
        public async Task UpdateItem(int id, ProjectDto item)
        {
            Project project = await _repository.GetById(id);
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
            await _repository.UpdateItem(_mapper.Map<Project>(project));
        }
    }
}
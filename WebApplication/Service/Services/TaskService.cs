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
            if (item == null) throw new ArgumentNullException("item");
            List<TaskItemDto> taskItems = new List<TaskItemDto>();
            taskItems = GetAll();
            foreach (TaskItemDto taskItem in taskItems)
            {
                if (taskItem.Title == item.Title)
                    throw new Exception("The Title is already exists");
            }
            item= DateTime.Now;
            return _mapper.Map<TaskItemDto>(_repository.AddItem(_mapper.Map<TaskItem>(item)));       
        }
       public void DeleteItem(int id)
        {
            TaskItem taskItem = _repository.GetById(id);
            if (taskItem == null) throw new ArgumentNullException(nameof(id));
            taskItem.Status = false;
            _repository.UpdateItem( user);
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
            _repository.UpdateItem(_mapper.Map<TaskItem>(item));
        }
    }
}

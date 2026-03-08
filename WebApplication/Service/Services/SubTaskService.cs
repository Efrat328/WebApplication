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
            if (item == null) throw new ArgumentNullException("item");
            List<SubTaskDto> subTasks = new List<SubTaskDto>();
            subTasks = GetAll();
            foreach (SubTaskDto subTask in subTasks)
            {
                if (subTask.Title == item.Title)
                    throw new Exception("This subTask is already exists");
            }
            return _mapper.Map<SubTaskDto>(_repository.AddItem(_mapper.Map<SubTask>(item)));
        }
        public void DeleteItem(int id)
        {
            SubTask subTask = _repository.GetById(id);
            if (subTask == null) throw new ArgumentNullException(nameof(id));
            subTask.Status = SubTaskStatus.Canceled;
            _repository.UpdateItem(subTask);
        }
        public List<SubTaskDto> GetAll()
        {
            return _mapper.Map<List<SubTaskDto>>(_repository.GetAll());
        }
        public SubTaskDto GetById(int id)
        {
            if(id==null) throw new ArgumentNullException(nameof(id));
            return _mapper.Map<SubTaskDto>(_repository.GetById(id));
        }
        public void UpdateItem(int id, SubTaskDto item)
        {
            SubTask subTask = _repository.GetById(id);
            if (subTask != null)
            {
                subTask.Title = item.Title;
                subTask.Description = item.Description;
                subTask.AssignedTo = item.AssignedTo;
                subTask.Deadline = item.Deadline;
                subTask.Status = item.Status;

            }
            else
            {
                throw new ArgumentNullException(nameof(id));
            }
            _repository.UpdateItem( _mapper.Map<SubTask>(item));
        }
    }
}
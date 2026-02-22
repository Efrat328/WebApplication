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
    public class HistoryService : IService<HistoryDto>
    {
        private readonly IRepository<History> _repository;
        private readonly IMapper _mapper;

        public HistoryService(IRepository<History> repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }
        public HistoryDto AddItem(HistoryDto item)
        {
            return _mapper.Map<HistoryDto>(_repository.AddItem(_mapper.Map<History>(item)));
        }
        public void DeleteItem(int id)
        {
            _repository.DeleteItem(id);
        }
        public List<HistoryDto> GetAll()
        {
            return _mapper.Map<List<HistoryDto>>(_repository.GetAll());
        }
        public HistoryDto GetById(int id)
        {
            return _mapper.Map<HistoryDto>(_repository.GetById(id));
        }
        public void UpdateItem(int id, HistoryDto item)
        {
            _repository.UpdateItem(id, _mapper.Map<History>(item));
        }
    }
}
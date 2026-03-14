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
        public async Task<HistoryDto> AddItem(HistoryDto item)
        {
            return _mapper.Map<HistoryDto>(await _repository.AddItem(_mapper.Map<History>(item)));
        }
        public async Task AddHistory(SubTaskStatus oldStatus, SubTaskStatus newStatus, int id)
        {
            HistoryDto historyDto = new HistoryDto
            {
                SubTaskId = id,
                OldStatus = (HistoryStatus)oldStatus, // Explicit cast to HistoryStatus
                NewStatus = (HistoryStatus)newStatus, // Explicit cast to HistoryStatus
                ChangedAt = DateTime.Now
            };
            await AddItem(historyDto);
        }
        public async Task DeleteItem(int id)
        {
            History history = await _repository.GetById(id);
            if (history == null) throw new ArgumentNullException(nameof(id));
            history.IsActive = false;
            await _repository.UpdateItem(history);
        }
        public async Task<List<HistoryDto>> GetAll()
        {
            return _mapper.Map<List<HistoryDto>>(await _repository.GetAll());
        }
        public async Task<HistoryDto> GetById(int id)
        {
            return _mapper.Map<HistoryDto>(await _repository.GetById(id));
        }
        public async Task UpdateItem(int id, HistoryDto item)
        {
            await _repository.UpdateItem( _mapper.Map<History>(item));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class HistoryRepository : IRepository<History>
    {
        private readonly IContext _context;
        public HistoryRepository(IContext context)
        {
            this._context = context;
        }
        public async Task<History> AddItem(History item)
        {
            await _context.Histories.AddAsync(item);
            await _context.SaveAsync();
            return item;
        }
        public async Task<List<History>> GetAll()
        {
            return await _context.Histories.ToListAsync();
        }
        public async Task<History> GetById(int id)
        {
            return await _context.Histories.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task UpdateItem(History item)
        {
            _context.Histories.Update(item);
            await _context.SaveAsync();
        }
    }
}

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
    public class SubTaskRepository : IRepository<SubTask>
    {
        private readonly IContext _context;
        public SubTaskRepository(IContext context)
        {
            this._context = context;
        }
        public async Task<SubTask> AddItem(SubTask item)
        {
            await _context.SubTasks.AddAsync(item);
            await _context.SaveAsync();
            return item;
        }
        public async Task<List<SubTask>> GetAll()
        {
            return await _context.SubTasks.ToListAsync();
        }
        public async Task<SubTask> GetById(int id)
        {
            return await _context.SubTasks.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task UpdateItem(SubTask item)
        {
            _context.SubTasks.Update(item);
            await _context.SaveAsync();
        }
    }
}

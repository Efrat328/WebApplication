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
    public class TaskItemRepository : IRepository<TaskItem>
    {
        private readonly IContext _context;
        public TaskItemRepository(IContext context)
        {
            this._context = context;
        }
        public async Task<TaskItem> AddItem(TaskItem item)
        {
            await _context.Tasks.AddAsync(item);
            await _context.SaveAsync();
            return item;
        }

        public async Task<List<TaskItem>> GetAll()
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.User)
                .ToListAsync();
        }
       
        public async Task<TaskItem> GetById(int id)
        {
            return await _context.Tasks
                .Include(t => t.SubTasks)
                .FirstOrDefaultAsync(x => x.Id == id);
}
       public async Task UpdateItem(TaskItem item)
        {
            _context.Tasks.Update(item);
            await _context.SaveAsync();
        }
    }
}
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
        public void DeleteItem(int id)
        {
            _context.Tasks.ToList().Remove(GetById(id));
            _context.Save();
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
        //public void UpdateItem(int id, TaskItem item)
        //{
        //    var task = GetById(id);
        //    task.Title = item.Title;
        //    task.Description = item.Description;
        //    task.Status = item.Status;
        //    task.Priority = item.Priority;
        //    task.Deadline = item.Deadline;
        //    _context.Save();
        //}
       public async Task UpdateItem(TaskItem item)
        {
            _context.Tasks.Update(item);
            await _context.SaveAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public TaskItem AddItem(TaskItem item)
        {
            _context.Tasks.ToList().Add(item);
            _context.Save();
            return item;
        }
        public void DeleteItem(int id)
        {
            _context.Tasks.ToList().Remove(GetById(id));
            _context.Save();
        }
        public List<TaskItem> GetAll()
        {
            return _context.Tasks.ToList();
        }
        public TaskItem GetById(int id)
        {
            return _context.Tasks.ToList().FirstOrDefault(x => x.Id == id);
        }
        public void UpdateItem(int id, TaskItem item)
        {
            var task = GetById(id);
            task.Title = item.Title;
            task.Description = item.Description;
            task.Status = item.Status;
            task.Priority = item.Priority;
            task.Deadline = item.Deadline;
            _context.Save();
        }
    }
}
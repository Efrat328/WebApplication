using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public SubTask AddItem(SubTask item)
        {
            _context.SubTasks.ToList().Add(item);
            _context.Save();
            return item;
        }
        public void DeleteItem(int id)
        {
            _context.SubTasks.ToList().Remove(GetById(id));
            _context.Save();
        }
        public List<SubTask> GetAll()
        {
            return _context.SubTasks.ToList();
        }
        public SubTask GetById(int id)
        {
            return _context.SubTasks.ToList().FirstOrDefault(x => x.Id == id);
        }
        public void UpdateItem(int id, SubTask item)
        {
            var subTask = GetById(id);
            subTask.Title = item.Title;
            subTask.Description = item.Description;
            subTask.Status = item.Status;
            subTask.Deadline = item.Deadline;
            _context.Save();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class ProjectRepository : IRepository<Project>
    {
        private readonly IContext _context;
        public ProjectRepository(IContext context)
        {
            this._context = context;
        }
        public Project AddItem(Project item)
        {
            _context.Projects.ToList().Add(item);
            _context.Save();
            return item;
        }
        public void DeleteItem(int id)
        {
            _context.Projects.ToList().Remove(GetById(id));
            _context.Save();
        }
        public List<Project> GetAll()
        {
            return _context.Projects.ToList();
        }
        public Project GetById(int id)
        {
            return _context.Projects.ToList().FirstOrDefault(x => x.Id == id);
        }
        public void UpdateItem(int id, Project item)
        {
            var project = GetById(id);
            project.NameProject = item.NameProject;
            project.Description = item.Description;
            project.Status = item.Status;
            project.Deadline = item.Deadline;
            _context.Save();
        }
    }
}
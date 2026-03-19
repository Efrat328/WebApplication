using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
        public async Task<Project> AddItem(Project item)
        {
            await _context.Projects.AddAsync(item);
            await _context.SaveAsync();
            return item;
        }
       
        public async Task<List<Project>> GetAll()
        {
            return await _context.Projects.ToListAsync();
        }
        public async Task<Project> GetById(int id)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task UpdateItem(Project item)
        {
            _context.Projects.Update(item);
            await _context.SaveAsync();
        }
    }
}
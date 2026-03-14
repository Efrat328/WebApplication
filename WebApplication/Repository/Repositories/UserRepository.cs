using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;
using Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly IContext _context;
        public UserRepository(IContext context)
        {
            this._context = context;
        }
        public async Task<User> AddItem(User item)
        {
            await _context.Users.AddAsync(item);
            await _context.SaveAsync();
            return item;
        }
        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task UpdateItem(User item)
        {
            _context.Users.Update(item);
            await _context.SaveAsync();
        }
    }
}

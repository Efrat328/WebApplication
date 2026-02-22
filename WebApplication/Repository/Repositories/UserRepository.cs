using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly IContext _context;
        public UserRepository(IContext context)
        {
            this._context = context;
        }
        public User AddItem(User item)
        {
            _context.Users.ToList().Add(item);
            _context.Save();
            return item;
        }
        public void DeleteItem(int id)
        {
            _context.Users.ToList().Remove(GetById(id));
            _context.Save();
        }
        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }
        public User GetById(int id)
        {
            return _context.Users.ToList().FirstOrDefault(x => x.Id == id);
        }
        public void UpdateItem(int id, User item)
        {
            var user = GetById(id);
            user.NameUser = item.NameUser;
            user.Email = item.Email;
            user.IsActive = item.IsActive;
            _context.Save();
        }
    }
}

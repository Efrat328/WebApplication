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
        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }
        public User GetById(int id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            return _context.Users.ToList().FirstOrDefault(x => x.Id == id);
        }
        public void UpdateItem( User item)
        {
            _context.Users.Update(item);
            _context.Save();
        }
    }
}

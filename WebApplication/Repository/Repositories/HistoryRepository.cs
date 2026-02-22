using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class HistoryRepository : IRepository<History>
    {
        private readonly IContext _context;
        public HistoryRepository(IContext context)
        {
            this._context = context;
        }
        public History AddItem(History item)
        {
            _context.Histories.ToList().Add(item);
            _context.Save();
            return item;
        }
        public void DeleteItem(int id)
        {
            _context.Histories.ToList().Remove(GetById(id));
            _context.Save();
        }
        public List<History> GetAll()
        {
            return _context.Histories.ToList();
        }
        public History GetById(int id)
        {
            return _context.Histories.ToList().FirstOrDefault(x => x.Id == id);
        }
        public void UpdateItem(int id, History item)
        {
            var history = GetById(id);
            history.OldStatus = item.OldStatus;
            history.NewStatus = item.NewStatus;
            history.ChangedAt = item.ChangedAt;
            _context.Save();
        }
    }
}

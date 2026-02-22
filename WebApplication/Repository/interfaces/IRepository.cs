using Repository.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRepository<T> : IRepositoryParent<T>
    {
        T GetById(int id);
        void UpdateItem(int id, T item);
        void DeleteItem(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRepository<T> : IRepositoryParent<T>
    {
        Task<T> GetById(int id);
        Task UpdateItem( T item);
        
    }
}

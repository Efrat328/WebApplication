using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRepositoryParent<T>
    {
        Task<List<T>> GetAll();
        Task<T> AddItem(T item);
    }
}

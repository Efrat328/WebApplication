using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IService<T> : IServiceParent<T>
    {
        Task<T> GetById(int id);
        Task UpdateItem(int id, T item);
        Task DeleteItem(int id);
    }
}

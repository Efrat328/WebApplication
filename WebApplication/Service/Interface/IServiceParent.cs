using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IServiceParent<T>
    {
        Task<List<T>> GetAll();
        Task<T> AddItem(T item);
    }
}

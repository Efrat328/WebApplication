using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IService<T> : IServiceParent<T>
    {
        T GetById(int id);
        void UpdateItem(int id, T item);
        void DeleteItem(int id);
    }
}

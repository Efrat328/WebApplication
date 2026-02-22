using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRepositoryParent<T>
    {
        List<T> GetAll();
        T AddItem(T item);
    }
}

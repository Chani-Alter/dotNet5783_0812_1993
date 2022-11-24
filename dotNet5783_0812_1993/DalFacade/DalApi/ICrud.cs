using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public interface ICrud<T>
    {
        int Add(T obj);
        void Delete(int id);
        void Update(T obj);
        T GetById(int id);
        T[] GetAll();
    }
}

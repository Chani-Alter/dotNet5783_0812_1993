using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public interface ICrud<T>
    {
        public int Add(T obj);
        public void Delete(int id);
        public void Update(T obj);
        public T GetById(int id);
        public IEnumerable<T> GetAll();
    }
}

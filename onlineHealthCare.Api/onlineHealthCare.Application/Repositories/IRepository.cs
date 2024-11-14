using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineHealthCare.Application.Repositories
{
   public interface IRepository<T> //where T : class
    {
        Task<List<T>> GetAll();
        public T? GetDetails(int id);
        Task Insert(T t);
        public void Update(T entity);
        public T Delete(int id);
    }
}

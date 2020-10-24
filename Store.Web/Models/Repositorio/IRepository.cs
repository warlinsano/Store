using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Web.Models.Repositorio
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        T GetById(int? id);

        T Add(T entity);

        T Remove(int? id);

        T Update(int id, T entity);

        bool Exists(int id);
    }
}

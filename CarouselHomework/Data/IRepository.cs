using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarouselHomework.Data
{
    public interface IRepository<T> where T : class
    {
        void Create(T entity);
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Remove(int id);
        void Update(T entity);
        Task<bool> SaveChanges();
    }
}

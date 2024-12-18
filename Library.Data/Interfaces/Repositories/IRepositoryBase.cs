using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Interfaces.Repositories
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        IQueryable<T> FindByConditionWithTracking(Expression<Func<T, bool>> expression);
        void Create(T entity);
        Task<T> CreateAsync(T entity);
        T Update(T entity);
        void Delete(T entity);
        void AddRange(ICollection<T> entities);
        Task AddRangeAsync(ICollection<T> entities);
        void RemoveRange(ICollection<T> entities);
        void Save();
        Task SaveAsync();
    }
}

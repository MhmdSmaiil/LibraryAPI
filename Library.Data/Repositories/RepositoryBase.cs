using Microsoft.EntityFrameworkCore;
using Library.Data.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected AppDbContext _appDbContext { get; set; }
        public RepositoryBase(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> FindAll()
        {
            return this._appDbContext.Set<T>().AsNoTracking();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this._appDbContext.Set<T>().AsNoTracking().Where(expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IQueryable<T> FindByConditionWithTracking(Expression<Func<T, bool>> expression)
        {
            return this._appDbContext.Set<T>().Where(expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Create(T entity)
        {
            this._appDbContext.Set<T>().Add(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> CreateAsync(T entity)
        {
            var trackedEntity = await this._appDbContext.Set<T>().AddAsync(entity);
            return trackedEntity.Entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Update(T entity)
        {
            var trackedEntity = this._appDbContext.Set<T>().Update(entity);
            return trackedEntity.Entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            this._appDbContext.Set<T>().Remove(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public void RemoveRange(ICollection<T> entities)
        {
            this._appDbContext.Set<T>().RemoveRange(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public void AddRange(ICollection<T> entities)
        {
            this._appDbContext.Set<T>().AddRange(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public async Task AddRangeAsync(ICollection<T> entities)
        {
            await this._appDbContext.Set<T>().AddRangeAsync(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task SaveAsync()
        {
            await this._appDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            this._appDbContext.SaveChanges();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using Infra.Data.Context;
using Domain.Interfaces.Repository;

namespace Infra.Data.Repository
{
	public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected AppDbContext Db;
        protected DbSet<T> DbSet;

        public BaseRepository(AppDbContext context)
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            Db = context;
            DbSet = Db.Set<T>();
        }

        public virtual T Add(T obj)
        {
            DbSet.Add(obj);
            return obj;
        }

        public T GetOne(object[] keys)
        {
            return DbSet.Find(keys);
        }

        public virtual IQueryable<T> GetAll(params string[] includeProperties)
        {

            var query = DbSet.AsNoTracking<T>().AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.AsNoTracking<T>();
        }

        public virtual T Update(T obj)
        {
            return DbSet.Update(obj).Entity;
        }

        public virtual void Remove(params object[] keys)
        {
            var entity = DbSet.Find(keys);
            if (entity != null)
            {
                DbSet.Remove(entity);
            }
        }

        public virtual void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate, params string[] includeProperties)
        {
            var query = DbSet.AsNoTracking<T>().Where(predicate);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.AsNoTracking<T>();
        }

        public int SaveChanges()
        {
            return Db.SaveChanges();
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
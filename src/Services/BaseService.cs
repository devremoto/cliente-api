using System;
using System.Linq.Expressions;
using System.Linq;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;

namespace Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected IBaseRepository<T> _baseRepository;

        public BaseService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public virtual T Add(T entity)
        {
            return _baseRepository.Add(entity);
        }

        public void Dispose()
        {
            _baseRepository.Dispose();
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate, params string[] includeProperties)
        {
            return _baseRepository.Find(predicate, includeProperties);
        }

        public virtual IQueryable<T> GetAll(params string[] includeProperties)
        {
            return _baseRepository.GetAll(includeProperties);
        }

        public virtual T GetOne(params object[] keys)
        {
            return _baseRepository.GetOne(keys);
        }

        public virtual void Remove(params object[] keys)
        {
            _baseRepository.Remove(keys);
        }

        public virtual void Remove(T entity)
        {
            _baseRepository.Remove(entity);
        }

        public int SaveChanges()
        {
            return _baseRepository.SaveChanges();
        }

        public virtual T Update(T entity)
        {
            return _baseRepository.Update(entity);
        }
    }
}

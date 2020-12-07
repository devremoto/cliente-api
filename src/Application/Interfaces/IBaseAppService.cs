using Application.ViewModels.Common;
using Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IBaseAppService<T, TViewModel>
        where T : class, IEntity
        where TViewModel : class, IViewModel, new()
    {
        TViewModel Add(TViewModel model);
        IEnumerable<TViewModel> Find(Expression<Func<TViewModel, bool>> predicate, params string[] includeProperties);
        IEnumerable<TViewModel> GetAll(params string[] includeProperties);
        TViewModel GetOne(params object[] keys);
        TViewModel Remove(TViewModel model);
        TViewModel Remove(params object[] keys);
        TViewModel Update(TViewModel model);
        TViewModel Save(TViewModel model);
        void Dispose();
    }
}
using Application.Interfaces;
using Application.ViewModels.Common;
using Domain.Interfaces;
using Domain.Interfaces.Services;
using System;
using System.Linq.Expressions;
using CrossCutting.Extensions;
using AutoMapper;
using System.Collections.Generic;
using Domain.Entities.Interfaces;

namespace Application.Services
{
    public class BaseAppService<T, TViewModel> : IBaseAppService<T, TViewModel>
        where T : class, IEntity
        where TViewModel : class, IViewModel, new()
    {
        private IUnitOfWork _uow;
        protected IBaseService<T> _baseService;
        private readonly IMapper _mapper;

        public BaseAppService(IBaseService<T> service, IMapper mapper, IUnitOfWork uow)
        {
            _uow = uow;
            _baseService = service;
            _mapper = mapper;
        }

        public IEnumerable<TViewModel> Find(Expression<Func<TViewModel, bool>> predicate, params string[] includeProperties)
        {
            var entity = _mapper.Map<Expression<Func<T, bool>>>(predicate);
            var model = _baseService.Find(entity, includeProperties);
            return _mapper.Map<IEnumerable<TViewModel>>(model);
        }

        public TViewModel GetOne(params object[] keys)
        {
            var model = _baseService.GetOne(keys);
            return _mapper.Map<TViewModel>(model);
        }

        public IEnumerable<TViewModel> GetAll(params string[] includeProperties)
        {
            var model = _baseService.GetAll(includeProperties);
            return _mapper.Map<IEnumerable<TViewModel>>(model);
        }


        public TViewModel Add(TViewModel model)
        {
            var validate = ValidateInsert(model);
            if (!validate.IsValid)
            {
                return validate;
            }
            var entity = _mapper.Map<T>(model);
            var result = _baseService.Add(entity);
            _uow.Commit();
            return _mapper.Map<TViewModel>(result);
        }

        public TViewModel Update(TViewModel model)
        {
            var validate = ValidateUpdate(model);
            if (!validate.IsValid)
            {
                return validate;
            }
            var entity = _mapper.Map<T>(model);
            var result = _baseService.Update(entity);
            _uow.Commit();
            return _mapper.Map<TViewModel>(result);
        }


        public TViewModel Save(TViewModel model)
        {
            var entity = _mapper.Map<T>(model);
            T result;
            if (entity.Id != null && entity.Id != Guid.Empty)
            {
                result = _baseService.Update(entity);
            }
            else
            {
                result = _baseService.Add(entity);
            }
            _uow.Commit();
            return _mapper.Map<TViewModel>(result);
        }

        public TViewModel Remove(TViewModel model)
        {
            var validate = ValidateDelete(model);
            if (!validate.IsValid)
            {
                return validate;
            }
            var entity = _mapper.Map<T>(model);
            _baseService.Remove(entity);
            _uow.Commit();
            return model;
        }

        public TViewModel Remove(params object[] key)
        {
            var validate = ValidateDelete(key);
            if (!validate.IsValid)
            {
                return validate;
            }
            _baseService.Remove(key);
            _uow.Commit();
            return new TViewModel();
        }

        public virtual TViewModel ValidateDelete(TViewModel model)
        {
            return model;
        }

        public virtual TViewModel ValidateDelete(params object[] key)
        {
            return new TViewModel();
        }

        public virtual TViewModel ValidateInsert(TViewModel model)
        {
            return model;
        }

        public virtual TViewModel ValidateUpdate(TViewModel model)
        {
            return model;
        }
        public void Dispose()
        {
            _uow.Dispose();
        }


    }

}

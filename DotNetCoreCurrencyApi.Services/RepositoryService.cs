using DotNetCoreCurrencyApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DotNetCoreCurrencyApi.Services
{
    public class RepositoryService<TRepository> : IBusinessLogicService
        where TRepository : IEntityGenericRepository
    {
        IEntityGenericRepository repository;
        public RepositoryService(TRepository repo)
        {
            repository = repo;
        }

        public void Create<TEntity>(TEntity entity, string createdBy = null) where TEntity : class
        {
            repository.Create<TEntity>(entity, createdBy);
        }

        public void Delete<TEntity>(object id) where TEntity : class
        {
            repository.Delete<TEntity>(id);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            repository.Delete<TEntity>(entity);
        }

        public IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int?
            skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            return repository.Get<TEntity>(filter, orderBy, skip, take, includes);
        }

        public IEnumerable<TEntity> GetAll<TEntity>(Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            return repository.GetAll<TEntity>(orderBy, skip, take, includes);
        }

        public Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            return repository.GetAllAsync<TEntity>(orderBy, skip, take, includes);
        }

        public Task<IQueryable<TEntity>> GetAllAsyncQueryable<TEntity>(Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            return repository.GetAllAsyncQueryable<TEntity>(orderBy, skip, take, includes);
        }

        public Task<IEnumerable<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity,
            bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            return repository.GetAsync<TEntity>(filter, orderBy, skip, take, includes);
        }

        public TEntity GetById<TEntity>(object id) where TEntity : class
        {
            return repository.GetById<TEntity>(id);
        }

        public Task<TEntity> GetByIdAsync<TEntity>(object id) where TEntity : class
        {
            return repository.GetByIdAsync<TEntity>(id);
        }

        public int GetCount<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            return repository.GetCount<TEntity>(filter);
        }

        public Task<int> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            return repository.GetCountAsync<TEntity>(filter);
        }

        public bool GetExists<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            return repository.GetExists<TEntity>(filter);
        }

        public Task<bool> GetExistsAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            return repository.GetExistsAsync<TEntity>(filter);
        }

        public TEntity GetFirst<TEntity>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            return repository.GetFirst<TEntity>(filter, orderBy, includes);
        }

        public Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            return repository.GetFirstAsync<TEntity>(filter, orderBy, includes);
        }

        public TEntity GetOne<TEntity>(Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            return repository.GetOne<TEntity>(filter, includes);
        }

        public Task<TEntity> GetOneAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            return repository.GetOneAsync<TEntity>(filter, includes);
        }

        public void Save()
        {
            repository.Save();
        }

        public async Task SaveAsync()
        {
            await repository.SaveAsync();
        }

        public void Update<TEntity>(TEntity entity, string modifiedBy = null) where TEntity : class
        {
            repository.Update<TEntity>(entity, modifiedBy);
        }
    }
}

using Maktoob.Domain.Entities;
using Maktoob.Domain.Repositories;
using Maktoob.Domain.Specifications;
using Maktoob.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Persistance.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : Entity<Guid>
    {
        protected readonly MaktoobDbContext _dbContext;

        public Repository(MaktoobDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbContext.AddAsync<TEntity>(entity);
        }

        public Task AddAsync(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().AddRange(entities);
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(TEntity entity)
        {
            _dbContext.Remove<TEntity>(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public virtual async Task<TEntity> GetAsync(SingleResultSpec<TEntity> spec)
        {
            var reslut = await _dbContext.Set<TEntity>().SingleOrDefaultAsync(spec.ToExpression());

            return reslut;
        }

        public virtual async Task<IList<TEntity>> GetAsync(MultiResultSpec<TEntity> spec)
        {
            var result = await _dbContext.Set<TEntity>().Where(spec.ToExpression()).ToListAsync();

            return result;
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            _dbContext.Update<TEntity>(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().UpdateRange(entities);
            return Task.CompletedTask;
        }
    }
}

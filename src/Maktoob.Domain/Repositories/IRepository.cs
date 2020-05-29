using Maktoob.Domain.Entities;
using Maktoob.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Domain.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : Entity<Guid>
    {
        IUnitOfWork UnitOfWork { get; }
        Task<IList<TEntity>> GetAsync(MultiResultSpec<TEntity> spec);
        Task<TEntity> GetAsync(SingleResultSpec<TEntity> spec);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task AddAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(IEnumerable<TEntity> entities);
    }

}

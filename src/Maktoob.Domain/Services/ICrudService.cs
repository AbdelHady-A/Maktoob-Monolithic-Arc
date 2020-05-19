using Maktoob.CrossCuttingConcerns.Error;
using Maktoob.CrossCuttingConcerns.Result;
using Maktoob.Domain.Entities;
using Maktoob.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Domain.Services
{
    public interface ICrudService<TEntity>
        where TEntity : Entity<Guid>
    {
        GErrorDescriber ErrorDescriber { get; }

        Task<GResult> CreateAsync(TEntity entity);
        Task<GResult> CreateAsync(IEnumerable<TEntity> entities);
        Task<GResult<TEntity>> ReadAsync(SingleResultSpec<TEntity> spec);
        Task<GResult<IEnumerable<TEntity>>> ReadAsync(MultiResultSpec<TEntity> spec);
        Task<GResult> UpdateAsync(TEntity entity);
        Task<GResult> UpdateAsync(IEnumerable<TEntity> entities);
        Task<GResult> DeleteAsync(TEntity entity);
        Task<GResult> DeleteAsync(IEnumerable<TEntity> entities);

    }
}

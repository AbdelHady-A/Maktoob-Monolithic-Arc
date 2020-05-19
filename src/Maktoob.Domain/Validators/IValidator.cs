using Maktoob.CrossCuttingConcerns.Error;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maktoob.Domain.Validators
{
    public interface IValidator<TEntity> where TEntity : class
    {
        Task<IEnumerable<GError>> ValidateAsync(TEntity entity);
    }
}

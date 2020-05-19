using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task CommitTransactionAsync();
    }
}

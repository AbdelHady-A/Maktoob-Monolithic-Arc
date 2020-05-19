using Maktoob.Domain.Repositories;
using Maktoob.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MaktoobDbContext _dbContext;
        private IDbContextTransaction _dbContextTransaction;

        public UnitOfWork(MaktoobDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel);
        }

        public async Task CommitTransactionAsync()
        {
            await _dbContextTransaction.CommitAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

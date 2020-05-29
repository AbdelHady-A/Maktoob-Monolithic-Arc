using Maktoob.Domain.Entities;
using Maktoob.Domain.Repositories;
using Maktoob.Domain.Specifications;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maktoob.Persistance.Repositories
{
    public class MongoRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity<Guid>
    {
        private readonly IMongoDatabase _database;

        public MongoRepository(IMongoDatabase database)
        {
            _database = database;
            UnitOfWork = new MongoUnitOfWork();
        }

        public IUnitOfWork UnitOfWork { get; }

        public async Task AddAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task AddAsync(IEnumerable<TEntity> entities)
        {
            await _collection.InsertManyAsync(entities);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await _collection.DeleteOneAsync(e => e.Id == entity.Id);
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await DeleteAsync(entity);
            }
        }

        public async Task<IList<TEntity>> GetAsync(MultiResultSpec<TEntity> spec)
        {
            var result = await _collection.AsQueryable().Where(spec.ToExpression()).ToListAsync();

            return result;
        }

        public async Task<TEntity> GetAsync(SingleResultSpec<TEntity> spec)
        {
            var result = await _collection.AsQueryable().FirstOrDefaultAsync(spec.ToExpression());
            
            return result;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity);
            }
        }

        private IMongoCollection<TEntity> _collection
            => _database.GetCollection<TEntity>(typeof(TEntity).FullName);
    }
}

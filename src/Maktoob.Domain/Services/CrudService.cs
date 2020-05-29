using Maktoob.CrossCuttingConcerns.Error;
using Maktoob.CrossCuttingConcerns.Result;
using Maktoob.Domain.Entities;
using Maktoob.Domain.Events;
using Maktoob.Domain.Repositories;
using Maktoob.Domain.Specifications;
using Maktoob.Domain.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Domain.Services
{
    public abstract class CrudService<TEntity> : ICrudService<TEntity>
        where TEntity : Entity<Guid>
    {

        public CrudService(IRepository<TEntity> repository, IEnumerable<IValidator<TEntity>> validators, GErrorDescriber errorDescriber)
        {
            _repository = repository;
            _validators = validators ?? new IValidator<TEntity>[0];
            ErrorDescriber = errorDescriber;
            _unitOfWork = this._repository.UnitOfWork;
        }

        protected readonly IRepository<TEntity> _repository;
        protected readonly IEnumerable<IValidator<TEntity>> _validators;

        public GErrorDescriber ErrorDescriber { get; }
        protected readonly IUnitOfWork _unitOfWork;

        public virtual async Task<GResult> CreateAsync(TEntity entity)
        {
            var errors = new List<GError>();
            foreach (var v in _validators)
            {
                var result = await v.ValidateAsync(entity);
                errors.AddRange(result);
            }

            if (errors.Count > 0)
            {
                return GResult.Failed(errors.ToArray());
            }
            await _repository.AddAsync(entity);
            try
            {
                await _unitOfWork.SaveChangesAsync();
                DomainEvents.Dispatch(new EntityCreatedEvent<TEntity>(entity));
                return GResult.Success;
            }
            catch
            {
                return GResult.Failed(ErrorDescriber.DefaultError());
            }
        }

        public virtual async Task<GResult> DeleteAsync(TEntity entity)
        {
            await _repository.DeleteAsync(entity);
            try
            {
                await _unitOfWork.SaveChangesAsync();
                DomainEvents.Dispatch(new EntityDeletedEvent<TEntity>(entity));
                return GResult.Success;
            }
            catch
            {
                return GResult.Failed(ErrorDescriber.DefaultError());
            }
        }

        public virtual async Task<GResult<TEntity>> ReadAsync(SingleResultSpec<TEntity> spec)
        {
            var result = await _repository.GetAsync(spec);
            if (result != null)
            {
                return GResult<TEntity>.Success(result);
            }
            else
            {
                return GResult<TEntity>.Failed(ErrorDescriber.NotFound());
            }
        }

        public virtual async Task<GResult<IEnumerable<TEntity>>> ReadAsync(MultiResultSpec<TEntity> spec)
        {
            var result = await _repository.GetAsync(spec);
            if (result != null || result?.Count > 0)
            {
                return GResult<IEnumerable<TEntity>>.Success(result);
            }
            else
            {
                return GResult<IEnumerable<TEntity>>.Failed(ErrorDescriber.NotFound());
            }
        }

        public virtual async Task<GResult> UpdateAsync(TEntity entity)
        {
            var errors = new List<GError>();
            foreach (var v in _validators)
            {
                var result = await v.ValidateAsync(entity);
                errors.AddRange(result);
            }

            if (errors.Count > 0)
            {
                return GResult.Failed(errors.ToArray());
            }
            await _repository.UpdateAsync(entity);
            try
            {
                await _unitOfWork.SaveChangesAsync();
                DomainEvents.Dispatch(new EntityUpdatedEvent<TEntity>(entity));
                return GResult.Success;
            }
            catch
            {
                return GResult.Failed(ErrorDescriber.DefaultError());
            }
        }

        public async Task<GResult> DeleteAsync(IEnumerable<TEntity> entities)
        {
            await _repository.DeleteAsync(entities);
            try
            {
                await _unitOfWork.SaveChangesAsync();
                DomainEvents.Dispatch(new EntitiesDeletedEvent<TEntity>(entities));
                return GResult.Success;
            }
            catch
            {
                return GResult.Failed(ErrorDescriber.DefaultError());
            }
        }

        public async Task<GResult> CreateAsync(IEnumerable<TEntity> entities)
        {
            var errors = new List<GError>();
            foreach (var entity in entities)
            {
                foreach (var v in _validators)
                {
                    var result = await v.ValidateAsync(entity);
                    errors.AddRange(result);
                }
            }
            if (errors.Count > 0)
            {
                return GResult.Failed(errors.ToArray());
            }
            await _repository.AddAsync(entities);
            try
            {
                await _unitOfWork.SaveChangesAsync();
                DomainEvents.Dispatch(new EntitiesCreatedEvent<TEntity>(entities));
                return GResult.Success;
            }
            catch
            {
                return GResult.Failed(ErrorDescriber.DefaultError());
            }
        }

        public async Task<GResult> UpdateAsync(IEnumerable<TEntity> entities)
        {
            var errors = new List<GError>();
            foreach (var entity in entities)
            {
                foreach (var v in _validators)
                {
                    var result = await v.ValidateAsync(entity);
                    errors.AddRange(result);
                }
            }
            
            if (errors.Count > 0)
            {
                return GResult.Failed(errors.ToArray());
            }
            await _repository.UpdateAsync(entities);
            try
            {
                await _unitOfWork.SaveChangesAsync();
                DomainEvents.Dispatch(new EntitiesUpdatedEvent<TEntity>(entities));
                return GResult.Success;
            }
            catch
            {
                return GResult.Failed(ErrorDescriber.DefaultError());
            }
        }
    }
}

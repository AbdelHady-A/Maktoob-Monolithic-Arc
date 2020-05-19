using Maktoob.CrossCuttingConcerns.Normalizers;
using Maktoob.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Maktoob.Domain.Specifications
{
    public abstract class SingleResultSpec<TEntity> : Specification<TEntity> {}

    public class FindByIdSpec<TEntity> : SingleResultSpec<TEntity>
        where TEntity : Entity<Guid>
    {
        public Guid Id { get;  }
        public FindByIdSpec(Guid id)
        {
            Id = id;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            return (entity) => entity.Id == Id;
        }
    }

    public class FindByNameSpec<TEntity> : SingleResultSpec<TEntity>
    where TEntity : EntityHasName<Guid>
    {
        public string Name { get;  }
        protected readonly IKeyNormalizer _keyNormalizer;

        public FindByNameSpec(string name, IKeyNormalizer keyNormalizer)
        {
            Name = name;
            _keyNormalizer = keyNormalizer;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            var normalizedName = _keyNormalizer.Normalize(Name);
            return (entity) => entity.NormalizedName == normalizedName;
        }
    }

    public class FindByEmailSpec<TEntity> : SingleResultSpec<TEntity>
        where TEntity : User
    {
        private readonly IKeyNormalizer _keyNormalizer;

        public FindByEmailSpec(string email, IKeyNormalizer keyNormalizer)
        {
            Email = email;
            _keyNormalizer = keyNormalizer;
        }

        public string Email { get; set; }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            var normalizedEmail = _keyNormalizer.Normalize(Email);
            return (entity) => entity.NormalizedEmail == normalizedEmail;
        }
    }

    public class FindUserLogin<TEntity> : SingleResultSpec<TEntity>
        where TEntity : UserLogin
    {
        public FindUserLogin(Guid userId, string jwtId, string refreshToken)
        {
            UserId = userId;
            JwtId = jwtId;
            RefreshToken = refreshToken;
        }
        public string JwtId { get; }
        public Guid UserId { get;  }
        public string RefreshToken { get;  }
        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            return (entity) => entity.UserId == UserId && entity.JwtId == JwtId && entity.RefreshToken == RefreshToken;
;        }
    }
}

using Maktoob.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Maktoob.Domain.Specifications
{
    public  abstract class MultiResultSpec<TEntitiy> : Specification<TEntitiy> {}

    public class FindUserLogins<TEntity> : MultiResultSpec<TEntity>
        where TEntity : UserLogin
    {
        public FindUserLogins(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; protected set; }
        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            return (entity) => entity.UserId == UserId;
        }
    }
}

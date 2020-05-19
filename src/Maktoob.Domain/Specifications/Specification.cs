using System;
using System.Linq;
using System.Linq.Expressions;

namespace Maktoob.Domain.Specifications
{

    public abstract class Specification<TEntity>
    {
        public bool IsSatisfiedBy(TEntity entity)
        {
            Func<TEntity, bool> predicate = ToExpression().Compile();

            return predicate(entity);
        }

        public abstract Expression<Func<TEntity, bool>> ToExpression();

        public Specification<TEntity> And(Specification<TEntity> specification)
        {
            return new AndSpecification<TEntity>(this, specification);
        }

        public Specification<TEntity> Or(Specification<TEntity> specification)
        {
            return new OrSpecification<TEntity>(this, specification);
        }

        public Specification<TEntity> Not()
        {
            return new NotSpecification<TEntity>(this);
        }
    }

    internal sealed class NotSpecification<TEntity> : Specification<TEntity>
    {
        private readonly Specification<TEntity> _specification;

        public NotSpecification(Specification<TEntity> specification)
        {
            _specification = specification;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            Expression<Func<TEntity, bool>> expression = _specification.ToExpression();
            UnaryExpression notExpression = Expression.Not(expression);

            return Expression.Lambda<Func<TEntity, bool>>(notExpression, expression.Parameters.Single());
        }
    }

    internal sealed class OrSpecification<TEntity> : Specification<TEntity>
    {
        private readonly Specification<TEntity> _left;
        private readonly Specification<TEntity> _right;

        public OrSpecification(Specification<TEntity> left, Specification<TEntity> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            Expression<Func<TEntity, bool>> leftExpression = _left.ToExpression();
            Expression<Func<TEntity, bool>> rightExpression = _right.ToExpression();

            BinaryExpression orExpression = Expression.OrElse(leftExpression.Body, rightExpression.Body);

            return Expression.Lambda<Func<TEntity, bool>>(orExpression, leftExpression.Parameters.Single());
        }
    }

    internal sealed class AndSpecification<TEntity> : Specification<TEntity>
    {
        private readonly Specification<TEntity> _left;
        private readonly Specification<TEntity> _right;

        public AndSpecification(Specification<TEntity> left, Specification<TEntity> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            Expression<Func<TEntity, bool>> leftExpression = _left.ToExpression();
            Expression<Func<TEntity, bool>> rightExpression = _right.ToExpression();

            BinaryExpression andExpression = Expression
                .AndAlso(leftExpression, rightExpression);

            return Expression.Lambda<Func<TEntity, bool>>(andExpression, leftExpression.Parameters.Single());
        }
    }
}

using System;
using System.Linq.Expressions;
using FastExpressionCompiler;

namespace Specifications
{
    public class SpecificationBase<T> : ISpecification<T>
    {
        private Func<T, bool> _function;

        private Func<T, bool> Function => _function
                                          ?? (_function = _predicate.CompileFast());

        private readonly Expression<Func<T, bool>> _predicate;

        protected SpecificationBase() { }

        public SpecificationBase(Expression<Func<T, bool>> predicate)
        {
            _predicate = predicate;
        }

        public bool IsSatisfiedBy(T entity)
        {
            return Function(entity);
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            return _predicate;
        }

        public ISpecification<T> And(ISpecification<T> spec)
        {
            return this & spec;
        }

        public ISpecification<T> Or(ISpecification<T> spec)
        {
            return this | spec;
        }

        public ISpecification<T> Negative()
        {
            return !this;
        }

        public static implicit operator Expression<Func<T, bool>>(SpecificationBase<T> spec)
        {
            return spec._predicate;
        }

        public static bool operator true(SpecificationBase<T> spec)
        {
            return false;
        }

        public static bool operator false(SpecificationBase<T> spec)
        {
            return false;
        }

        public static SpecificationBase<T> operator !(SpecificationBase<T> spec)
        {
            return new SpecificationBase<T>(
                Expression.Lambda<Func<T, bool>>(
                    Expression.Not(spec._predicate.Body),
                    spec._predicate.Parameters));
        }

        public static SpecificationBase<T> operator &(SpecificationBase<T> left, ISpecification<T> right)
        {
            var leftExpr = left._predicate;
            var rightExpr = right.ToExpression();
            var leftParam = leftExpr.Parameters[0];
            var rightParam = rightExpr.Parameters[0];

            return new SpecificationBase<T>(
                Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(
                        leftExpr.Body,
                        new ParameterReplacer(rightParam, leftParam).Visit(rightExpr.Body) ??
                        throw new NullReferenceException()),
                    leftParam));
        }

        public static SpecificationBase<T> operator |(SpecificationBase<T> left, ISpecification<T> right)
        {
            var leftExpr = left._predicate;
            var rightExpr = right.ToExpression();
            var leftParam = leftExpr.Parameters[0];
            var rightParam = rightExpr.Parameters[0];

            return new SpecificationBase<T>(
                Expression.Lambda<Func<T, bool>>(
                    Expression.OrElse(
                        leftExpr.Body,
                        new ParameterReplacer(rightParam, leftParam).Visit(rightExpr.Body) ?? 
                        throw new NullReferenceException()),
                    leftParam));
        }
    }
}
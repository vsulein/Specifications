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

        public bool IsSatisfiedBy(T entity) => Function(entity);

        public Expression<Func<T, bool>> ToExpression() => _predicate;

        public ISpecification<T> And(ISpecification<T> spec)
        {
            return new SpecificationBase<T>(_predicate.Compose(spec.ToExpression(), Expression.AndAlso));
        }

        public ISpecification<T> Or(ISpecification<T> spec)
        {
            return new SpecificationBase<T>(_predicate.Compose(spec.ToExpression(), Expression.OrElse));
        }

        public ISpecification<T> Negative()
        {
            return new SpecificationBase<T>(
                Expression.Lambda<Func<T, bool>>(
                    Expression.Not(_predicate.Body),
                    _predicate.Parameters));
        }

        public static implicit operator Expression<Func<T, bool>>(SpecificationBase<T> spec) => spec._predicate;

        public static implicit operator Func<T, bool>(SpecificationBase<T> spec) => spec.Function;

        public static bool operator true(SpecificationBase<T> spec)
        {
            return false;
        }

        public static bool operator false(SpecificationBase<T> spec)
        {
            return false;
        }

        public static SpecificationBase<T> operator !(SpecificationBase<T> spec) => (SpecificationBase<T>)spec.Negative();

        public static SpecificationBase<T> operator &(SpecificationBase<T> left, SpecificationBase<T> right) => (SpecificationBase<T>)left.And(right);

        public static SpecificationBase<T> operator |(SpecificationBase<T> left,  SpecificationBase<T> right) => (SpecificationBase<T>)left.Or(right);
    }
}
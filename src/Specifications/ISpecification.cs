using System;
using System.Linq.Expressions;

namespace Specifications
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T obj);

        Expression<Func<T, bool>> ToExpression();

        ISpecification<T> And(ISpecification<T> spec);

        ISpecification<T> Or(ISpecification<T> spec);
        
        ISpecification<T> Not();
    }
}
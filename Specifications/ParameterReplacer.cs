using System.Linq.Expressions;

namespace Specifications
{
    internal class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;
        private readonly ParameterExpression _replacement;

        public ParameterReplacer(ParameterExpression parameter, ParameterExpression replacement)
        {
            _parameter = parameter;
            _replacement = replacement;
        }

        protected override Expression VisitParameter(ParameterExpression paramExpr)
        {
            return base.VisitParameter(_parameter == paramExpr ? _replacement : paramExpr);
        }
    }
}
using System;
using System.Linq.Expressions;

namespace APPartment.ORM.Framework.Helpers
{
    public static class ExpressionToSqlHelper
    {
        public static string GetWhereClause<T>(Expression<Func<T, bool>> expression, object businessObject)
        {
            return GetValueAsString(expression.Body, businessObject);
        }

        private static string GetValueAsString(Expression expression, object businessObject)
        {
            var value = "";
            var equalty = "";
            var left = GetLeftNode(expression);
            var right = GetRightNode(expression);
            if (expression.NodeType == ExpressionType.Equal)
            {
                equalty = "=";
            }
            if (expression.NodeType == ExpressionType.AndAlso)
            {
                equalty = "AND";
            }
            if (expression.NodeType == ExpressionType.OrElse)
            {
                equalty = "OR";
            }
            if (expression.NodeType == ExpressionType.NotEqual)
            {
                equalty = "<>";
            }
            if (left is MemberExpression)
            {
                var leftMem = left as MemberExpression;
                value = string.Format("({0}{1}'{2}')", leftMem.Member.Name, equalty, "{0}");
            }
            if (right is ConstantExpression)
            {
                var rightConst = right as ConstantExpression;
                value = string.Format(value, rightConst.Value);
            }
            if (right is MemberExpression)
            {
                var rightMem = right as MemberExpression;
                object val = null;

                try
                {
                    val = businessObject.GetType().GetProperty(rightMem.Member.Name).GetValue(businessObject);
                }
                catch (Exception)
                {
                    // TODO: Handle case when right member is not part of the businessObject's properties. Is a variable for example...
                }

                value = string.Format(value, val);
            }
            if (value == "")
            {
                var leftVal = GetValueAsString(left, businessObject);
                var rigthVal = GetValueAsString(right, businessObject);
                value = string.Format("({0} {1} {2})", leftVal, equalty, rigthVal);
            }
            return value;
        }

        private static Expression GetLeftNode(Expression expression)
        {
            dynamic exp = expression;
            return ((Expression)exp.Left);
        }

        private static Expression GetRightNode(Expression expression)
        {
            dynamic exp = expression;
            return ((Expression)exp.Right);
        }
    }
}

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NCheck
{
    /// <summary>
    /// Extension methods to unwrap expressions.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Unwrap an Expression to get to the appropriate PropertyInfo.
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<TU, TValue>(this Expression<Func<TU, TValue>> expression)
        {
            var me = GetMemberExpression(expression);
            return me.Member as PropertyInfo;
        }

        /// <summary>
        /// Unwrap an Expression to determine the appropriate MemberExpression.
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberExpression GetMemberExpression<TU, TValue>(this Expression<Func<TU, TValue>> expression)
        {
            if (expression == null)
            {
                return null;
            }

            var me = expression.Body as MemberExpression;
            if (me != null)
            {
                return me;
            }

            var ue = expression.Body as UnaryExpression;
            if (ue != null)
            {
                var operand = ue.Operand;
                var memberExpression = operand as MemberExpression;
                if (memberExpression != null)
                {
                    return memberExpression;
                }

                var callExpression = operand as MethodCallExpression;
                if (callExpression != null)
                {
                    return callExpression.Object as MemberExpression;
                }
            }

            return null;
        }
    }
}

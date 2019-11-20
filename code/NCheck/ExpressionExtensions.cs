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

            switch (expression.Body)
            {
                case MemberExpression me:
                    return me;

                case UnaryExpression ue:
                {
                    var operand = ue.Operand;
                    switch (operand)
                    {
                        case MemberExpression memberExpression:
                            return memberExpression;

                        case MethodCallExpression callExpression:
                            return callExpression.Object as MemberExpression;
                    }

                    break;
                }
            }

            return null;
        }
    }
}

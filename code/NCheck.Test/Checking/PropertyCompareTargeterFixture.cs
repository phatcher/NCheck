namespace NCheck.Test.Checking
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using NCheck.Checking;

    using NUnit.Framework;

    [TestFixture]
    public class PropertyCompareTargeterFixture
    {
        [Test]
        public void DefaultIsUnknown()
        {
            var targeter = new PropertyCompareTargeter();
            CheckTargetType<SampleClass>(targeter, x => x.Ignore, CompareTarget.Unknown);
        }

        [Test]
        public void DetermineValueBasedOnName()
        {
            var targeter = new PropertyCompareTargeter();
            targeter.Register(x => x.Name == "Ignore" ? CompareTarget.Ignore : CompareTarget.Unknown);

            CheckTargetType<SampleClass>(targeter, x => x.Ignore, CompareTarget.Ignore);  
        }

        private void CheckTargetType<T>(PropertyCompareTargeter targeter, Expression<Func<T, object>> propertyExpression, CompareTarget target)
        {
            var pi = GetPropertyInfo(propertyExpression);
            Assert.AreEqual(target, targeter.DetermineCompareTarget(pi)); 
        }

        /// <summary>
        /// Unwrap an Expression to get to the appropriate PropertyInfo.
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        private PropertyInfo GetPropertyInfo<TU, TValue>(Expression<Func<TU, TValue>> expression)
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
        private MemberExpression GetMemberExpression<TU, TValue>(Expression<Func<TU, TValue>> expression)
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
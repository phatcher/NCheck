using System;
using System.Linq.Expressions;

using NCheck.Checking;

using NUnit.Framework;

namespace NCheck.Test.Checking
{
    [TestFixture]
    public class PropertyConventionsFixture
    {
        [Test]
        public void DefaultIsUnknown()
        {
            var targeter = new PropertyConventions();
            CheckTargetType<SampleClass>(targeter, x => x.Ignore, CompareTarget.Unknown);
        }

        [Test]
        public void DetermineValueBasedOnName()
        {
            var targeter = new PropertyConventions();
            targeter.CompareTarget.Register(x => x.Name == "Ignore", CompareTarget.Ignore);

            CheckTargetType<SampleClass>(targeter, x => x.Ignore, CompareTarget.Ignore);  
        }

        private void CheckTargetType<T>(PropertyConventions targeter, Expression<Func<T, object>> propertyExpression, CompareTarget target)
        {
            var pi = propertyExpression.GetPropertyInfo();
            Assert.AreEqual(target, targeter.CompareTarget.Convention(pi)); 
        }
    }
}
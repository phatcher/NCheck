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
            var candidate = new PropertyConventions();
            CheckTargetType<SampleClass>(candidate, x => x.Ignore, CompareTarget.Unknown);
        }

        [Test]
        public void DetermineValueBasedOnName()
        {
            var candidate = new PropertyConventions();
            candidate.CompareTarget.Register(x => x.Name == "Ignore", CompareTarget.Ignore);

            CheckTargetType<SampleClass>(candidate, x => x.Ignore, CompareTarget.Ignore);  
        }

        private void CheckTargetType<T>(PropertyConventions candidate, Expression<Func<T, object>> propertyExpression, CompareTarget target)
        {
            var pi = propertyExpression.GetPropertyInfo();
            Assert.That(candidate.CompareTarget.Convention(pi), Is.EqualTo(target)); 
        }
    }
}
using System;
using System.Linq;

using NCheck.Checking;

using NUnit.Framework;

namespace NCheck.Test
{
    [TestFixture]
    public class PropertyCheckExpressionFixture : Fixture
    {
        private PropertyCheck pc;
        private PropertyCheckExpression pce;

        [Test]
        public void IdAssignedNotCompatible()
        {
            try
            {
                pce.Id();
            }
            catch (NotSupportedException)
            {                
            }
        }

        [Test]
        public void EntityAssigned()
        {
            pce.Entity();

            Assert.That(pc.CompareTarget, Is.EqualTo(CompareTarget.Entity));
        }

        [Test]
        public void IgnoreAssigned()
        {
            pce.Ignore();

            Assert.That(pc.CompareTarget, Is.EqualTo(CompareTarget.Ignore));
        }

        [Test]
        public void CountAssigned()
        {
            pce.Count();

            Assert.That(pc.CompareTarget, Is.EqualTo(CompareTarget.Count));
        }

        [Test]
        public void LengthAssigned()
        {
            pce.Length(20);

            Assert.That(pc.Length, Is.EqualTo(20));
        }

        [Test]
        public void ValueAssigned()
        {
            pce.Value();

            Assert.That(pc.CompareTarget, Is.EqualTo(CompareTarget.Value));
        }

        protected override void OnSetup()
        {
            var x = CheckerFactory;
            var checker = new SimpleChecker() as ICheckerInitializer;
            pc = checker.Properties.First();
            pce = new PropertyCheckExpression(pc);
        }
    }
}
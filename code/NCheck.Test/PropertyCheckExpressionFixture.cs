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

            Assert.AreEqual(CompareTarget.Entity, pc.CompareTarget);
        }

        [Test]
        public void IgnoreAssigned()
        {
            pce.Ignore();

            Assert.AreEqual(CompareTarget.Ignore, pc.CompareTarget);
        }

        [Test]
        public void CountAssigned()
        {
            pce.Count();

            Assert.AreEqual(CompareTarget.Count, pc.CompareTarget);
        }

        [Test]
        public void LengthAssigned()
        {
            pce.Length(20);

            Assert.AreEqual(20, pc.Length);
        }

        [Test]
        public void ValueAssigned()
        {
            pce.Value();

            Assert.AreEqual(CompareTarget.Value, pc.CompareTarget);
        }

        protected override void OnSetup()
        {
            var x = CheckerFactory;
            var checker = new SimpleChecker() as ICheckerCompare;
            pc = checker.Properties.First();
            pce = new PropertyCheckExpression(pc);
        }
    }
}
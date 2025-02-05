using NCheck.Checking;

using NUnit.Framework;

namespace NCheck.Test
{
    [TestFixture]
    public class AutoBuildConventionsFixture : Fixture
    {
        [Test]
        public void ShouldObeyIgnoreConvention()
        {
            CheckProperty("Ignore", CompareTarget.Ignore);
        }

        [Test]
        public void AutoShouldObeyIgnoreConvention()
        {
            CheckerProperty<Parent>("Ignore", CompareTarget.Ignore);
        }

        [Test]
        public void ShouldObeyExplicitIgnore()
        {
            CheckProperty("Another", CompareTarget.Ignore);
        }

        [Test]
        public void AutoShouldNotObeyExplicitIgnore()
        {
            CheckerProperty<Parent>("Another", CompareTarget.Value);
        }

        private void CheckerProperty<T>(string name, CompareTarget expected)
        {
            var cf = CheckerFactory as CheckerFactory;
            cf.Clear();

            var checker = cf.CheckerHelper<T>() as Checker<T>;
            var property = checker[name];
            Assert.That(property.CompareTarget, Is.EqualTo(expected));
        }

        private void CheckProperty(string name, CompareTarget expected)
        {
            var checker = new ParentChecker();
            var property = checker[name];
            Assert.That(property.CompareTarget, Is.EqualTo(expected));
        }

        protected override void OnSetup()
        {
            // Force initialization of the checker factory
            var x = CheckerFactory;
        }
    }
}
namespace NCheck.Test
{
    using NCheck.Checking;

    using NUnit.Framework;

    [TestFixture]
    public class BuilderConventionsFixture : Fixture
    {
        [Test]
        public void ShouldObeyIgnoreConvention()
        {
            CheckProperty<Parent>("Ignore", CompareTarget.Ignore);
        }

        [Test]
        public void ShouldObeyTypeConvention()
        {
            CheckProperty<Child>("Parent", CompareTarget.Id);
        }

        [Test]
        public void StandardPropertyShouldBeValue()
        {
            CheckProperty<Parent>("Another", CompareTarget.Value);
        }

        private void CheckProperty<T>(string name, CompareTarget expected)
        {
            var builder = new CheckerBuilder(CheckerFactory);
            var checker = (Checker<T>) builder.Build(typeof(T));
            var property = checker[name];
            Assert.AreEqual(expected, property.CompareTarget);
        }
    }
}

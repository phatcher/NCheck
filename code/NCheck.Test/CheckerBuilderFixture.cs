using NUnit.Framework;

namespace NCheck.Test
{
    [TestFixture]
    public class CheckerBuilderFixture : Fixture
    {
        protected override ICheckerFactory CreateCheckerFactory()
        {
            return new CheckerFactory();
        }

        [Test]
        public void BuildSimplePropertyChecker()
        {
            var builder = new CheckerBuilder(CheckerFactory);

            var checker = (ICheckerInitializer) builder.Build(typeof(Fred));

            Assert.AreEqual(1, checker.Properties.Count, "Property count differs");
        }

        [Test]
        public void BuildDescendantPropertyChecker()
        {
            var builder = new CheckerBuilder(CheckerFactory);

            var checker = (ICheckerInitializer)builder.Build(typeof(Jim));

            Assert.AreEqual(1, checker.Properties.Count, "Property count differs");
        }

        private class Fred
        {
            public int Id { get; set; }
        }

        private class Jim : Fred
        {
            public string Name { get; set; }
        }
    }
}
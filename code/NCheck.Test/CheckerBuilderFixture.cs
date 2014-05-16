namespace NCheck.Test
{
    using NCheck.Checking;

    using NUnit.Framework;

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

            var checker = (ICheckerCompare) builder.Build(typeof(Fred));

            Assert.AreEqual(1, checker.Properties.Count, "Property count differs");
        }

        [Test]
        public void BuildDescendantPropertyChecker()
        {
            var builder = new CheckerBuilder(CheckerFactory);

            var checker = (ICheckerCompare)builder.Build(typeof(Jim));

            Assert.AreEqual(1, checker.Properties.Count, "Property count differs");
        }

        protected override void OnSetup()
        {
            base.OnSetup();

            PropertyCheck.IdentityChecker = new IdentifiableChecker();
            PropertyCheck.Targeter = new TypeCompareTargeter();
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
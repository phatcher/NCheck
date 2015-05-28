namespace NCheck.Test.Examples
{
    using NUnit.Framework;

    [TestFixture]
    public class SimpleTest
    {
        [Test]
        public void AlgoTest()
        {
            var checkerFactory = new CheckerFactory();

            var algo = new ShinyBusinessService();
            var source = new Simple { Id = 1, Name = "A", Value = 1.0 };

            var expected = new Simple { Id = 2, Name = "B", Value = 1.2 };

            var candidate = algo.Run(source);
            checkerFactory.Check(expected, candidate);
        }
    }
}
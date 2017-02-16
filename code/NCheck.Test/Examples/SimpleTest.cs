using NCheck.Checking;

using NUnit.Framework;

namespace NCheck.Test.Examples
{
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

        [Test]
        public void CompareDoubleAbsComparerTest()
        {
            var checkerFactory = new CheckerFactory();

            var expected = new Simple { Id = 1, Name = "A", Value = 10.00005 };

            var candidate = new Simple { Id = 1, Name = "A", Value = 10.00006 };

            checkerFactory.Check(expected, candidate);
        }

        [Test]
        public void CompareDoubleStandardComparerTest()
        {
            var checkerFactory = new CheckerFactory();
            PropertyCheck.TypeConventions.Comparer.Clear();

            var expected = new Simple { Id = 1, Name = "A", Value = 10.00005 };

            var candidate = new Simple { Id = 1, Name = "A", Value = 10.00006 };

            try
            {
                checkerFactory.Check(expected, candidate);
            }
            catch (PropertyCheckException pex)
            {
                Assert.That(pex.Message, Is.EqualTo("Simple.Value: Expected:<10.00005>. Actual:<10.00006>"), "Message differs");
            }
        }

        [Test]
        public void GenericTest()
        {
            var checkerFactory = new CheckerFactory();

            var algo = new ShinyBusinessService();
            var source = new Simple {Id = 1, Name = "A", Value = 1.0};

            var expected = new Simple {Id = 2, Name = "B", Value = 1.2};

            var candidate = algo.Run(source);

            var wexpected = new Wrapper<Simple> {Content = expected};
            var wcandidate = new Wrapper<Simple> {Content = candidate};
            checkerFactory.Check(wexpected, wcandidate);
        }

        [Test]
        public void GenericInterfaceTest()
        {
            var checkerFactory = new CheckerFactory();

            var algo = new ShinyBusinessService();
            var source = new Simple { Id = 1, Name = "A", Value = 1.0 };

            var expected = new Simple { Id = 2, Name = "B", Value = 1.2 };

            var candidate = algo.Run(source);

            IWrapper<Simple> wexpected = new Wrapper<Simple> { Content = expected };
            IWrapper<Simple> wcandidate = new Wrapper<Simple> { Content = candidate };
            checkerFactory.Check(wexpected, wcandidate);
        }
    }
}
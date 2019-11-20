using NCheck.Checking;
using NCheck.Test.Checking;

using NUnit.Framework;

namespace NCheck.Test
{
    [TestFixture]
    public class DictionaryStringCheckFixture : Fixture
    {
        [Test]
        public void NullExpected()
        {
            var expected = new SampleDictionary
            {
                Strings = null
            };

            var candidate = new SampleDictionary();

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Strings: Expected:<null>. Actual:<not null>"), "Message differs");
        }

        [Test]
        public void NullCandidate()
        {
            var expected = new SampleDictionary();

            var candidate = new SampleDictionary
            {
                Strings = null
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Strings: Expected:<not null>. Actual:<null>"), "Message differs");
        }

        [Test]
        public void MissingValueExpected()
        {
            var expected = new SampleDictionary();

            var candidate = new SampleDictionary
            {
                Strings =
                {
                    ["A"] = "A"
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Strings\r\n[A]: Expected:<null>. Actual:<A>"), "Message differs");
        }

        [Test]
        public void MissingValueCandidate()
        {
            var expected = new SampleDictionary
            {
                Strings =
                {
                    ["A"] = "A"
                }
            };

            var candidate = new SampleDictionary();

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Strings\r\n[A]: Expected:<A>. Actual:<null>"), "Message differs");
        }

        [Test]
        public void DifferentValue()
        {
            var expected = new SampleDictionary
            {
                Strings =
                {
                    ["A"] = "A",
                    ["B"] = "B"
                }
            };

            var candidate = new SampleDictionary
            {
                Strings =
                {
                    ["A"] = "A",
                    ["B"] = "A"
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Strings\r\n[B]: Expected:<B>. Actual:<A>"), "Message differs");
        }

        [Test]
        public void MultipleDifferences()
        {
            var expected = new SampleDictionary
            {
                Strings =
                {
                    ["A"] = "A",
                    ["B"] = "B",
                    ["C"] = "C"
                }
            };

            var candidate = new SampleDictionary
            {
                Strings =
                {
                    ["A"] = "A",
                    ["B"] = "A",
                    ["D"] = "D"
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Strings\r\n[B]: Expected:<B>. Actual:<A>\r\n[C]: Expected:<C>. Actual:<null>\r\n[D]: Expected:<null>. Actual:<D>"), "Message differs");
        }
    }
}
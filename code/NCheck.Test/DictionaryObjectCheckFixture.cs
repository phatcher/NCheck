using NCheck.Checking;
using NCheck.Test.Checking;

using NUnit.Framework;

namespace NCheck.Test
{
    [TestFixture]
    public class DictionaryObjectCheckFixture : Fixture
    {
        [Test]
        public void NullExpected()
        {
            var expected = new SampleDictionary
            {
                Properties = null
            };

            var candidate = new SampleDictionary();

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Properties: Expected:<null>. Actual:<not null>"), "Message differs");
        }

        [Test]
        public void NullCandidate()
        {
            var expected = new SampleDictionary();

            var candidate = new SampleDictionary
            {
                Properties = null
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Properties: Expected:<not null>. Actual:<null>"), "Message differs");
        }

        [Test]
        public void MissingValueExpected()
        {
            var expected = new SampleDictionary();

            var candidate = new SampleDictionary
            {
                Properties =
                {
                    ["A"] = 1
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Properties\r\n[A]: Expected:<null>. Actual:<1>"), "Message differs");
        }

        [Test]
        public void MissingValueCandidate()
        {
            var expected = new SampleDictionary
            {
                Properties =
                {
                    ["A"] = 1
                }
            };

            var candidate = new SampleDictionary();

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Properties\r\n[A]: Expected:<1>. Actual:<null>"), "Message differs");
        }

        [Test]
        public void DifferentValue()
        {
            var expected = new SampleDictionary
            {
                Properties =
                {
                    ["A"] = 1,
                    ["B"] = 2
                }
            };

            var candidate = new SampleDictionary
            {
                Properties =
                {
                    ["A"] = 1,
                    ["B"] = 1
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Properties\r\n[B]: Expected:<2>. Actual:<1>"), "Message differs");
        }

        [Test]
        public void MultipleDifferences()
        {
            var expected = new SampleDictionary
            {
                Properties =
                {
                    ["A"] = 1,
                    ["B"] = 2,
                    ["C"] = 3
                }
            };

            var candidate = new SampleDictionary
            {
                Properties =
                {
                    ["A"] = 1,
                    ["B"] = 1,
                    ["D"] = 4
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Properties\r\n[B]: Expected:<2>. Actual:<1>\r\n[C]: Expected:<3>. Actual:<null>\r\n[D]: Expected:<null>. Actual:<4>"), "Message differs");
        }
    }
}
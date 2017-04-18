using System.Collections.Generic;
using NCheck.Checking;
using NCheck.Test.Checking;

using NUnit.Framework;

namespace NCheck.Test
{
    [TestFixture]
    public class DictionaryCheckFixture : Fixture
    {
        [Test]
        public void NullExpected()
        {
            var expected = new SampleDictionary
            {
                Properties = null
            };

            var candidate = new SampleDictionary
            {
                Properties = new Dictionary<string, object>()
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Properties: Expected:<null>. Actual:<not null>"), "Message differs");
        }

        [Test]
        public void NullCandidate()
        {
            var expected = new SampleDictionary
            {
                Properties = new Dictionary<string, object>()
            };

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
            var expected = new SampleDictionary
            {
                Properties = new Dictionary<string, object>()
            };

            var candidate = new SampleDictionary
            {
                Properties = new Dictionary<string, object>
                {
                    ["A"] = 1
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Properties differences...\r\nA: Expected:<null>. Actual:<1>\r\n"), "Message differs");
        }

        [Test]
        public void MissingValueCandidate()
        {
            var expected = new SampleDictionary
            {
                Properties = new Dictionary<string, object>
                {
                    ["A"] = 1
                }
            };

            var candidate = new SampleDictionary
            {
                Properties = new Dictionary<string, object>()
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Properties differences...\r\nA: Expected:<1>. Actual:<null>\r\n"), "Message differs");
        }

        [Test]
        public void DifferentValue()
        {
            var expected = new SampleDictionary
            {
                Properties = new Dictionary<string, object>
                {
                    ["A"] = 1,
                    ["B"] = 2
                }
            };

            var candidate = new SampleDictionary
            {
                Properties = new Dictionary<string, object>
                {
                    ["A"] = 1,
                    ["B"] = 1
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Properties differences...\r\nB: Expected:<2>. Actual:<1>\r\n"), "Message differs");
        }

        [Test]
        public void MultipleDifferences()
        {
            var expected = new SampleDictionary
            {
                Properties = new Dictionary<string, object>
                {
                    ["A"] = 1,
                    ["B"] = 2,
                    ["C"] = 1
                }
            };

            var candidate = new SampleDictionary
            {
                Properties = new Dictionary<string, object>
                {
                    ["A"] = 1,
                    ["B"] = 1,
                    ["D"] = 1
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Properties differences...\r\nB: Expected:<2>. Actual:<1>\r\nC: Expected:<1>. Actual:<null>\r\nD: Expected:<null>. Actual:<1>\r\n"), "Message differs");
        }
    }
}
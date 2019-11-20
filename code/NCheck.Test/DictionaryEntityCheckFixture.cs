using NCheck.Checking;
using NCheck.Test.Checking;

using NUnit.Framework;

namespace NCheck.Test
{
    [TestFixture]
    public class DictionaryEntityCheckFixture : Fixture
    {
        [Test]
        public void NullExpected()
        {
            var expected = new SampleDictionary
            {
                Children = null
            };

            var candidate = new SampleDictionary();

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Children: Expected:<null>. Actual:<not null>"), "Message differs");
        }

        [Test]
        public void NullCandidate()
        {
            var expected = new SampleDictionary();

            var candidate = new SampleDictionary
            {
                Children = null
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Children: Expected:<not null>. Actual:<null>"), "Message differs");
        }

        [Test]
        public void MissingValueExpected()
        {
            var expected = new SampleDictionary();

            var candidate = new SampleDictionary
            {
                Children =
                {
                    ["A"] = new SampleClass{ Id = 1 }
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Children\r\n[A]: Expected:<null>. Actual:<NCheck.Test.Checking.SampleClass>"), "Message differs");
        }

        [Test]
        public void MissingValueCandidate()
        {
            var expected = new SampleDictionary
            {
                Children =
                {
                    ["A"] = new SampleClass{ Id = 1 }
                }
            };

            var candidate = new SampleDictionary();

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Children\r\n[A]: Expected:<NCheck.Test.Checking.SampleClass>. Actual:<null>"), "Message differs");
        }

        [Test]
        public void DifferentValue()
        {
            var expected = new SampleDictionary
            {
                Children =
                {
                    ["A"] = new SampleClass{ Id = 1 },
                    ["B"] = new SampleClass{ Id = 2 }
                }
            };

            var candidate = new SampleDictionary
            {
                Children =
                {
                    ["A"] = new SampleClass{ Id = 1 },
                    ["B"] = new SampleClass{ Id = 1 }
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Children\r\n[B].Id: Expected:<2>. Actual:<1>"), "Message differs");
        }

        [Test]
        public void MultipleDifferences()
        {
            var expected = new SampleDictionary
            {
                Children =
                {
                    ["A"] = new SampleClass{ Id = 1 },
                    ["B"] = new SampleClass{ Id = 2 },
                    ["C"] = new SampleClass{ Id = 3 },
                }
            };

            var candidate = new SampleDictionary
            {
                Children =
                {
                    ["A"] = new SampleClass{ Id = 1 },
                    ["B"] = new SampleClass{ Id = 1 },
                    ["D"] = new SampleClass{ Id = 4 },
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Children\r\n[B].Id: Expected:<2>. Actual:<1>\r\n[C]: Expected:<NCheck.Test.Checking.SampleClass>. Actual:<null>\r\n[D]: Expected:<null>. Actual:<NCheck.Test.Checking.SampleClass>"), "Message differs");
        }
    }
}
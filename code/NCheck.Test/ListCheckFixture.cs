using NCheck.Checking;
using NCheck.Test.Checking;

using NUnit.Framework;

namespace NCheck.Test
{
    [TestFixture]
    public class ListCheckFixture : Fixture
    {
        [Test]
        public void NullExpected()
        {
            var expected = new SampleList
            {
                Children = null
            };

            var candidate = new SampleList();

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleList.Children: Expected:<null>. Actual:<not null>"), "Message differs");
        }

        [Test]
        public void NullCandidate()
        {
            var expected = new SampleList();

            var candidate = new SampleList
            {
                Children = null
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleList.Children: Expected:<not null>. Actual:<null>"), "Message differs");
        }

        [Test]
        public void MissingValueExpected()
        {
            var expected = new SampleList();

            var candidate = new SampleList
            {
                Children =
                {
                    new SampleClass{ Id = 1 }
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleList.Children\r\nCount: Expected:<0>. Actual:<1>"), "Message differs");
        }

        [Test]
        public void MissingValueCandidate()
        {
            var expected = new SampleList
            {
                Children =
                {
                    new SampleClass{ Id = 1 }
                }
            };

            var candidate = new SampleList();

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleList.Children\r\nCount: Expected:<1>. Actual:<0>"), "Message differs");
        }

        [Test]
        public void DifferentValue()
        {
            var expected = new SampleList
            {
                Children =
                {
                    new SampleClass{ Id = 1 },
                    new SampleClass{ Id = 2 }
                }
            };

            var candidate = new SampleList
            {
                Children =
                {
                    new SampleClass{ Id = 1 },
                    new SampleClass{ Id = 1 }
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleList.Children\r\n[1].Id: Expected:<2>. Actual:<1>"), "Message differs");
        }

        [Test]
        public void ExpectedCardinalityHigher()
        {
            var expected = new SampleList
            {
                Children =
                {
                    new SampleClass{ Id = 1 },
                    new SampleClass{ Id = 2 },
                    new SampleClass{ Id = 3 },
                }
            };

            var candidate = new SampleList
            {
                Children =
                {
                    new SampleClass{ Id = 1 },
                    new SampleClass{ Id = 4 },
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleList.Children\r\nCount: Expected:<3>. Actual:<2>\r\n[1].Id: Expected:<2>. Actual:<4>"), "Message differs");
        }

        [Test]
        public void ExpectedCardinalityLower()
        {
            var expected = new SampleList
            {
                Children =
                {
                    new SampleClass{ Id = 1 },
                }
            };

            var candidate = new SampleList
            {
                Children =
                {
                    new SampleClass{ Id = 1 },
                    new SampleClass{ Id = 4 },
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleList.Children\r\nCount: Expected:<1>. Actual:<2>"), "Message differs");
        }

        [Test]
        public void MultipleDifferences()
        {
            var expected = new SampleList
            {
                Children =
                {
                    new SampleClass{ Id = 1 },
                    new SampleClass{ Id = 2 },
                    new SampleClass{ Id = 3 },
                }
            };

            var candidate = new SampleList
            {
                Children =
                {
                    new SampleClass{ Id = 1 },
                    new SampleClass{ Id = 1 },
                    new SampleClass{ Id = 4 },
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleList.Children\r\n[1].Id: Expected:<2>. Actual:<1>\r\n[2].Id: Expected:<3>. Actual:<4>"), "Message differs");
        }
    }
}
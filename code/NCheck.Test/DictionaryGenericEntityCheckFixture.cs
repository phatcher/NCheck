using NCheck.Checking;
using NCheck.Test.Checking;

using NUnit.Framework;

namespace NCheck.Test
{
    [TestFixture]
    public class DictionaryGenericEntityCheckFixture : Fixture
    {
        [Test]
        public void NullExpected()
        {
            var expected = new SampleDictionary
            {
                Measures = null
            };

            var candidate = new SampleDictionary();

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Measures: Expected:<null>. Actual:<not null>"), "Message differs");
        }

        [Test]
        public void NullCandidate()
        {
            var expected = new SampleDictionary();

            var candidate = new SampleDictionary
            {
                Measures = null
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Measures: Expected:<not null>. Actual:<null>"), "Message differs");
        }

        [Test]
        public void MissingValueExpected()
        {
            var expected = new SampleDictionary();

            var candidate = new SampleDictionary
            {
                Measures =
                {
                    ["A"] = new Measure<decimal>{ Value = 1, Units = "m" }
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Measures\r\n[A]: Expected:<null>. Actual:<NCheck.Test.Checking.Measure`1[System.Decimal]>"), "Message differs");
        }

        [Test]
        public void MissingValueCandidate()
        {
            var expected = new SampleDictionary
            {
                Measures =
                {
                    ["A"] = new Measure<decimal>{ Value = 1, Units = "m" }
                }
            };

            var candidate = new SampleDictionary();

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Measures\r\n[A]: Expected:<NCheck.Test.Checking.Measure`1[System.Decimal]>. Actual:<null>"), "Message differs");
        }

        [Test]
        public void DifferentValue()
        {
            var expected = new SampleDictionary
            {
                Measures =
                {
                    ["A"] = new Measure<decimal>{ Value = 1, Units = "m" },
                    ["B"] = new Measure<decimal>{ Value = 2, Units = "m" }
                }
            };

            var candidate = new SampleDictionary
            {
                Measures =
                {
                    ["A"] = new Measure<decimal>{ Value = 1, Units = "m" },
                    ["B"] = new Measure<decimal>{ Value = 1, Units = "m" }
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Measures\r\n[B].Value: Expected:<2>. Actual:<1>"), "Message differs");
        }

        [Test]
        public void MultipleDifferences()
        {
            var expected = new SampleDictionary
            {
                Measures =
                {
                    ["A"] = new Measure<decimal>{ Value = 1, Units = "m" },
                    ["B"] = new Measure<decimal>{ Value = 2, Units = "m" },
                    ["C"] = new Measure<decimal>{ Value = 2, Units = "m" },
                }
            };

            var candidate = new SampleDictionary
            {
                Measures =
                {
                    ["A"] = new Measure<decimal>{ Value = 1, Units = "m" },
                    ["B"] = new Measure<decimal>{ Value = 1, Units = "m" },
                    ["D"] = new Measure<decimal>{ Value = 4, Units = "m" },
                }
            };

            var ex = Assert.Throws<PropertyCheckException>(() => Check(expected, candidate));
            Assert.That(ex.Message, Is.EqualTo("SampleDictionary.Measures\r\n[B].Value: Expected:<2>. Actual:<1>\r\n[C]: Expected:<NCheck.Test.Checking.Measure`1[System.Decimal]>. Actual:<null>\r\n[D]: Expected:<null>. Actual:<NCheck.Test.Checking.Measure`1[System.Decimal]>"), "Message differs");
        }
    }
}
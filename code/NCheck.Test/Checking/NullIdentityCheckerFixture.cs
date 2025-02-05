using System;
using System.Collections.Generic;

using NCheck.Checking;

using NUnit.Framework;

namespace NCheck.Test.Checking
{
    [TestFixture]
    public class NullIdentityCheckerFixture
    {
        [TestCase(typeof(SampleClass))]
        [TestCase(typeof(SampleEnum))]
        [TestCase(typeof(SampleStruct))]
        public void IdNotSupported(Type type)
        {
            var ic = new NullIdentityChecker();

            Assert.That(ic.SupportsId(type), Is.False);
        }

        [TestCaseSource(nameof(Samples))]
        public void ExtractReturnsNull(object value)
        {
            var ic = new NullIdentityChecker();

            Assert.That(ic.ExtractId(value), Is.Null);
        }

        public static IEnumerable<object> Samples
        {
            get
            {
                yield return new SampleClass();
                yield return new SampleStruct();
                yield return SampleEnum.KnownKnown;
            }
        }
    }
}
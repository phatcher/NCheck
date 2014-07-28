﻿namespace NCheck.Test.Checking
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using NCheck.Checking;

    using NUnit.Framework;

    [TestFixture]
    public class NullIdentityCheckerFixture
    {
        [TestCase(typeof(SampleClass))]
        [TestCase(typeof(SampleEnum))]
        [TestCase(typeof(SampleStruct))]
        public void IdNotSupported(Type type)
        {
            var ic = new NullIdentityChecker();

            Assert.IsFalse(ic.SupportsId(type));
        }

        [TestCaseSource("Samples")]
        public void ExtractReturnsNull(object value)
        {
            var ic = new NullIdentityChecker();

            Assert.IsNull(ic.ExtractId(value));
        }

        public IEnumerable<object> Samples
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
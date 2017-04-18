using System;
using System.Collections;
using System.Collections.Generic;

using NCheck.Checking;

using NUnit.Framework;

namespace NCheck.Test.Checking
{
    [TestFixture]
    public class TypeConventionsFixture
    {
        private TypeConventions conventions;

        [TestCase(typeof(Guid))]
        [TestCase(typeof(string))]
        [TestCase(typeof(short))]
        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        [TestCase(typeof(short?))]
        [TestCase(typeof(int?))]
        [TestCase(typeof(long?))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(float))]
        [TestCase(typeof(double))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(DateTimeOffset))]
        [TestCase(typeof(TimeSpan))]
        [TestCase(typeof(TimeZone))]
        [TestCase(typeof(TimeZoneInfo))]
        [TestCase(typeof(Type))]
        [TestCase(typeof(SampleStruct?))]
        [TestCase(typeof(SampleEnum))]
        public void ValueCompare(Type type)
        {
            Assert.That(conventions.CompareTarget.Convention(type), Is.EqualTo(CompareTarget.Value),  "Incorrect CompareTarget for " + type.Name);
        }

        [Test]
        public void StandardComparer()
        {
            Assert.That(conventions.Comparer.Convention(typeof(float)), Is.Null, "Incorrect Comparer for Single");
        }

        [Test]
        public void OverrrideComparer()
        {
            conventions.ComparerConvention<float>(AbsFloat);

            Assert.That(conventions.Comparer.Convention(typeof(float)), Is.Not.Null, "Incorrect Comparer for Single");
        }

        [TestCase(typeof(SampleStruct))]
        public void EntityCompare(Type type)
        {
            Assert.That(conventions.CompareTarget.Convention(type), Is.EqualTo(CompareTarget.Entity), "Incorrect CompareTarget for " + type.Name);    
        }

        [TestCase(typeof(List<int>))]
        [TestCase(typeof(IList<int>))]
        [TestCase(typeof(ICollection<int>))]
        public void CollectionCompare(Type type)
        {
            Assert.That(conventions.CompareTarget.Convention(type), Is.EqualTo(CompareTarget.Collection), "Incorrect CompareTarget for " + type.Name);
        }

        [TestCase(typeof(IDictionary))]
        [TestCase(typeof(IDictionary<string, object>))]
        [TestCase(typeof(Dictionary<string, object>))]
        public void DictionaryCompare(Type type)
        {
            Assert.That(conventions.CompareTarget.Convention(type), Is.EqualTo(CompareTarget.Dictionary), "Incorrect CompareTarget for " + type.Name);
        }

        [Test]
        public void CheckerFactoryRegisterTypeViaGeneric()
        {
            var cf = new CheckerFactory();
            cf.Convention<SampleClass>(CompareTarget.Ignore);

            Assert.That(PropertyCheck.TypeConventions.CompareTarget.Convention(typeof(SampleClass)), Is.EqualTo(CompareTarget.Ignore));
        }

        [Test]
        public void UseFunctionToDetermineCompareType()
        {
            conventions.CompareTarget.Register(x => typeof(IIdentifiable).IsAssignableFrom(x), CompareTarget.Id);
            Assert.AreEqual(CompareTarget.Id, conventions.CompareTarget.Convention(typeof(SampleClass)));
        }

        [SetUp]
        protected void OnSetup()
        {
            conventions = new TypeConventions();
            conventions.InitializeTypeConventions();
        }

        private bool AbsDouble(double x, double y)
        {
            return Math.Abs(x - y) < 0.001;
        }

        private bool AbsFloat(float x, float y)
        {
            return Math.Abs(x - y) < 0.001;
        }
    }
}
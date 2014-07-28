namespace NCheck.Test.Checking
{
    using System;
    using System.Collections.Generic;

    using NCheck.Checking;

    using NUnit.Framework;

    [TestFixture]
    public class TypeConventionsFixture
    {
        private TypeConventions conventions;

        [TestCase(typeof(Guid))]
        [TestCase(typeof(String))]
        [TestCase(typeof(Int16))]
        [TestCase(typeof(Int32))]
        [TestCase(typeof(Int64))]
        [TestCase(typeof(Int16?))]
        [TestCase(typeof(Int32?))]
        [TestCase(typeof(Int64?))]
        [TestCase(typeof(Double))]
        [TestCase(typeof(Single))]
        [TestCase(typeof(Double))]
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
            Assert.AreEqual(CompareTarget.Value, conventions.CompareTarget.Convention(type), "Incorrect CompareTarget for " + type.Name);
        }

        [TestCase(typeof(SampleStruct))]
        public void EntityCompare(Type type)
        {
            Assert.AreEqual(CompareTarget.Entity, conventions.CompareTarget.Convention(type), "Incorrect CompareTarget for " + type.Name);    
        }

        [TestCase(typeof(List<int>))]
        [TestCase(typeof(IList<int>))]
        [TestCase(typeof(ICollection<int>))]
        public void CollectionCompare(Type type)
        {
            Assert.AreEqual(CompareTarget.Collection, conventions.CompareTarget.Convention(type), "Incorrect CompareTarget for " + type.Name);
        }

        [Test]
        public void CheckerFactoryRegisterTypeViaGeneric()
        {
            var cf = new CheckerFactory();
            cf.Convention<SampleClass>(CompareTarget.Ignore);

            Assert.AreEqual(CompareTarget.Ignore, PropertyCheck.TypeConventions.CompareTarget.Convention(typeof(SampleClass)));
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
    }
}
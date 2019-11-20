using System.Collections.Generic;
using System.Linq;

using NCheck.Checking;

using NUnit.Framework;

namespace NCheck.Test
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class CheckingFixture : Fixture
    {
        [Test]
        public void ValidateAllComparedPropertiesAreSame()
        {
            var expected = new Simple { Id = 1, Name = "A" };
            var candidate = new Simple { Id = 1, Name = "A" };

            Check(expected, candidate);
        }

        [Test]
        public void ValidateDifferentIntPropertyMessage()
        {
            var expected = new Simple { Id = 1, Name = "A" };
            var candidate = new Simple { Id = 2, Name = "A" };

            CheckFault(expected, candidate, "Simple.Id", 1, 2);
        }

        [Test]
        public void ValidateDifferentStringPropertyMessage()
        {
            var expected = new Simple { Id = 1, Name = "A" };
            var candidate = new Simple { Id = 1, Name = "B" };

            CheckFault(expected, candidate, "Simple.Name", "A", "B");
        }

        [Test]
        public void ValidateCompareValueWithCustomComparer()
        {
            var expected = new Simple { Id = 1, Value = 1 };
            var candidate = new Simple { Id = 1, Value = 1.0005 };

            Check(expected, candidate);
        }

        [Test]
        public void ValidateCompareValueWithCustomComparerFault()
        {
            var expected = new Simple { Id = 1, Value = 1 };
            var candidate = new Simple { Id = 1, Value = 1.1 };

            CheckFault(expected, candidate, "Simple.Value", "1", "1.1");
        }

        [Test]
        public void ValidateCompareById()
        {
            var p1 = new Parent {Id = 1, Name = "A"};
            var expected = new Child {Id = 1, Name = "Child", Parent = p1};
            p1.Favourite = expected;

            var p2 = new Parent {Id = 1, Name = "A"};
            var candidate = new Child {Id = 1, Name = "Child", Parent = p2};
            p2.Favourite = candidate;

            Check(expected, candidate);
        }

        [Test]
        public void ValidateCompareByIdWithNullIdentityChecker()
        {
            var p1 = new Parent {Id = 1, Name = "A"};
            var expected = new Child {Id = 1, Name = "Child", Parent = p1};
            p1.Favourite = expected;

            var p2 = new Parent {Id = 1, Name = "A"};
            var candidate = new Child {Id = 1, Name = "Child", Parent = p2};
            p2.Favourite = candidate;

            // NB Need to initialize the checker factory
            var x = CheckerFactory;
            ConventionsFactory.Conventions.IdentityChecker = null;

            CheckFault(expected, candidate, "No IdentityChecker assigned, cannot perform Id check");
        }

        [Test]
        public void ValidateCompareByIdMessage()
        {
            var p1 = new Parent {Id = 1, Name = "A"};
            var expected = new Child {Id = 1, Name = "Child", Parent = p1};
            p1.Favourite = expected;

            var p2 = new Parent {Id = 2, Name = "A"};
            var candidate = new Child {Id = 1, Name = "Child", Parent = p2};
            p2.Favourite = candidate;

            CheckFault(expected, candidate, "Child.Parent.Id", 1, 2);
        }

        [Test]
        public void ValidateExcludeProperty()
        {
            var expected = new Parent {Id = 1, Name = "A", Ignore = "A", Another = 1};
            var candidate = new Parent {Id = 1, Name = "A", Ignore = "B", Another = 2};

            Check(expected, candidate);
        }

        [Test]
        public void ValidatePropertyMessageInChildEntity()
        {
            var expected = new Parent {Id = 1, Name = "A"};
            var c1 = new Child {Id = 1, Name = "Child", Parent = expected};
            expected.Favourite = c1;

            var candidate = new Parent {Id = 1, Name = "A"};
            var c2 = new Child {Id = 2, Name = "Child", Parent = candidate};
            candidate.Favourite = c2;

            CheckFault(expected, candidate, "Parent.Favourite.Id", 1, 2);
        }

        [Test]
        public void ValidateEntityWithList()
        {
            var c1 = new Child();
            var expected = new Parent {Id = 1, Name = "A", Children = new List<Child>()};
            var candidate = new Parent {Id = 1, Name = "A", Children = new List<Child>()};

            expected.Children.Add(c1);
            candidate.Children.Add(c1);

            Check(expected, candidate);
        }

        [Test]
        public void ValidateListExplicitly()
        {
            var c1 = new Child();
            var expected = new Parent {Id = 1, Name = "A", Children = new List<Child>()};
            var candidate = new Parent {Id = 1, Name = "A", Children = new List<Child>()};

            expected.Children.Add(c1);
            candidate.Children.Add(c1);

            Check<Child>(expected.Children, candidate.Children);
        }

        [Test]
        public void ValidateNullIIdentifiable()
        {
            var expected = new Child {Id = 1, Name = "Child"};
            var candidate = new Child {Id = 1, Name = "Child"};

            Check(expected, candidate);
        }

        [Test]
        public void ValidateWhenBothCollectionPropertyAreNull()
        {
            var expected = new Parent {Id = 1, Name = "A"};
            var candidate = new Parent {Id = 1, Name = "A"};

            Check(expected, candidate);
        }

        [Test]
        public void ValidateMessageWhenCandidateCollectionPropertyIsNull()
        {
            var expected = new Parent {Id = 1, Name = "A", Children = new List<Child>()};
            var candidate = new Parent {Id = 1, Name = "A"};

            CheckFault(expected, candidate, "Parent.Children", "not null", "null");
        }

        [Test]
        public void ValidateMessageWhenExpectedCollectionPropertyIsNull()
        {
            var expected = new Parent {Id = 1, Name = "A"};
            var candidate = new Parent {Id = 1, Name = "A", Children = new List<Child>()};

            CheckFault(expected, candidate, "Parent.Children", "null", "not null");
        }

        [Test]
        public void ValidateMessageWhenCandidateCollectionCountIsDifferent()
        {
            var c1 = new Child();
            var expected = new Parent {Id = 1, Name = "A", Children = new List<Child>()};
            var candidate = new Parent {Id = 1, Name = "A", Children = new List<Child>()};

            expected.Children.Add(c1);

            CheckFault(expected, candidate, "Parent.Children\r\nCount", 1, 0);
        }

        [Test]
        public void ValidateMessageWhenExpectedCollectionCountIsDifferent()
        {
            var c1 = new Child();
            var expected = new Parent {Id = 1, Name = "A", Children = new List<Child>()};
            var candidate = new Parent {Id = 1, Name = "A", Children = new List<Child>()};

            candidate.Children.Add(c1);

            CheckFault(expected, candidate, "Parent.Children\r\nCount", 0, 1);
        }

        [Test]
        public void ValidateMessageCollectionElementIsDifferent()
        {
            var expected = new Parent {Id = 1, Name = "A", Children = new List<Child>()};
            var candidate = new Parent {Id = 1, Name = "A", Children = new List<Child>()};

            var c1 = new Child {Id = 1};
            expected.Children.Add(c1);
            var c2 = new Child {Id = 2};
            candidate.Children.Add(c2);

            CheckFault(expected, candidate, "Parent.Children\r\n[0].Id", 1, 2);
        }

        [Test]
        public void ValidateUniquePropertyInfoChecked()
        {
            var checker = new SimpleChecker() as ICheckerInitializer;

            // Just grab one
            var expected = checker.Properties.ToList()[0];

            // Add it a again.
            var candidate = checker.Compare(expected.Info);

            Assert.AreSame(expected, candidate.PropertyCheck);
        }

        [Test]
        public void NoIIdentifiableCheckerRegistered()
        {
            var cf = (CheckerFactory) CheckerFactory;
            cf.Clear();
            cf.Conventions.IdentityChecker = null;

            var p1 = new Parent {Id = 1, Name = "A"};
            var expected = new Child {Id = 1, Name = "Child", Parent = p1};
            var candidate = new Child {Id = 2, Name = "Child", Parent = p1};

            CheckFault(expected, candidate, "No IdentityChecker assigned, cannot perform Id check");
        }

        [Test]
        public void NullBuilderMeansNoAutoRegistration()
        {
            var cf = (CheckerFactory) CheckerFactory;
            cf.Clear();
            cf.Builder = new NullCheckerBuilder();

            var p1 = new Parent {Id = 1, Name = "A"};
            var expected = new Child {Id = 1, Name = "Child", Parent = p1};
            var candidate = new Child {Id = 2, Name = "Child", Parent = p1};

            CheckFault(expected, candidate, "No checker registered for NCheck.Test.Child");
        }
        [Test]
        public void DefaultComparer()
        {
            var candidate = new Parent { Id = 1, Name = "A", Another = 1 };
            var expected = new Parent { Id = 1, Name = "A", Another = 2, };

            Check(expected, candidate);
        }

        [Test]
        public void IncludeAnotherProperty()
        {
            var expected = new Parent { Id = 1, Name = "A", Another = 1, };
            var candidate = new Parent { Id = 1, Name = "A", Another = 1, };

            Compare<Parent>(x => x.Another).Value();
            Check(expected, candidate);
        }

        [Test]
        public void IncludeAnotherPropertyComparisonFails()
        {
            var expected = new Parent { Id = 1, Name = "A", Another = 1, };
            var candidate = new Parent { Id = 1, Name = "A", Another = 2, };

            Compare<Parent>(x => x.Another).Value();
            CheckFault(expected, candidate, "Parent.Another", 1, 2);
        }

        [Test]
        public void ExcludeNameProperty()
        {
            var expected = new Parent { Id = 1, Name = "B", Another = 2 };
            var candidate = new Parent { Id = 1, Name = "A", Another = 1, };

            Compare<Parent>(x => x.Name).Ignore();
            Check(expected, candidate);
        }

        [Test]
        public void ValidateInvalidCast()
        {
            var expected = new Parent { Id = 1, Name = "A", Castable = 10 };
            var candidate = new Parent { Id = 1, Name = "A", Castable = 10M, };

            CheckFault(expected, candidate, "Parent.Castable: Could not cast candidate value 10 (Decimal) to Int32");
        }
    }
}
NCheck
======

Library providing object checking features primarily used for state based testing.

Available as a NuGet package [NCheck](https://www.nuget.org/packages/NCheck/)

Welcome to contributions from anyone.

You can see the version history [here](RELEASE_NOTES.md).

## Build the project
* Windows: Run *build.cmd*

I have my tools in C:\Tools so I use *build.cmd Default tools=C:\Tools encoding=UTF-8*

You can also get the package directly from NuGet

[![NuGet](https://img.shields.io/nuget/v/NCheck.svg)](https://www.nuget.org/packages/NCheck/)
[![Build status](https://ci.appveyor.com/api/projects/status/6y5wmxh8u93baffu/branch/master?svg=true)](https://ci.appveyor.com/project/PaulHatcher/csvreader/branch/master)

## Library License
The library is available under the [MIT License](http://en.wikipedia.org/wiki/MIT_License), for more information see the [License file][1] in the GitHub repository.

 [1]: https://github.com/phatcher/NCheck/License.md

## Getting Started
When writing unit tests it is important not to write brittle tests that fail when the underlying code changes.

To do this, developers write "black-box" tests which do not rely on the knowledge of the code, but just on the intended behaviour. This is also known as state based testing as it just checks the state before and after the execution of the system under test.

This is easy using xUnit style testing e.g. given a little class
```csharp
    public class Simple
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }
    }
```

We can have a simple test that invokes some business logic and checks the result
```csharp
    [TestFixture]
    public class SimpleTest
    {
        [Test]
        public void AlgoTest()
        {
            var algo = new ShinyBusinssService();
            var source = new Simple { Id = 1, Name = "A", Value = 1.0 } ;

            var candidate = algo.Run(source);
            Assert.AreEqual(2, candidate.Id, "Id differs");
            Assert.AreEqual("B", candidate.Name, "Name differs");
            Assert.AreEqual(1.2, candidate.Value, "Value differs");
        }
    }
```
The problem arises when things change, say we add two more properties to Simple and these are affected by our ShinyBusinessService.

Unfortunately, our test still passes since it does not know about the new properties and so ignores them, so we will typically pass our unit test phase and will only find out at integration testing that we have forgotten to take account of the new state.

Our solution is to introduce some helper classes and methods into the tests which will catch this type of error during unit testing (much cheaper!).

If we re-write the test using the NCheck library we get the following
```csharp
    [TestFixture]
    public class SimpleTest
    {
        [Test]
        public void AlgoTest()
        {
            var checkerFactory = new CheckerFactory();

            var algo = new ShinyBusinessService();
            var source = new Simple { Id = 1, Name = "A", Value = 1.0 } ;

            var expected = new Simple { Id = 2, Name = "B", Value = 1.2 } ;

            var candidate = algo.Run(source);
            checkerFactory.Check(expected, candidate);
        }
    }
```
Now when the definition of Simple and the algorithm changes, this test will fail because the expected value will not match since it does not define values for the new properties.

Couple of things to note here...

1. We have gone from comparing individual properties to comparing objects
2. By default we don't need to create a checker for an object, the library does this for us automatically
3. You typically subclass CheckerFactory to customize start up behaviour, and put a property in your test fixtures to create it on first use.


## Other testing scenarios

### Customizing the CheckerFactory

The CheckerFactory has a number conventions which are used to automatically construct Checkers for each class; these conventions can be overridden by the developer if
they don't suit a particular scenario.

We support...

* Type conventions: Applied to all instances of a particular type
* Property conventions: Applied to properties which satisfy a function e.g. matching a particular name
* Comparer conventions: Operates on a type or property convention and changes Value comparison from object.Equals to a specified comparer; useful for values such putting error bound on floating point comparison.

```csharp
    public class CheckerFactory : NCheck.CheckerFactory
    {
        public CheckerFactory()
        {
            PropertyCheck.IdentityChecker = new IdentifiableChecker();

            Initialize();
        }

        private void Initialize()
        {
            // Set us up as the global factory, used to locate the checkers later on.
            // Also done by the builder, but this is needed if the builder is not used.
            NCheck.Checker.CheckerFactory = this;

            // NB Conventions must be before type registrations if they are to apply.
            Convention(x => typeof(IIdentifiable).IsAssignableFrom(x), CompareTarget.Id);
            Convention((PropertyInfo x) => x.Name == "Ignore", CompareTarget.Ignore);

            // NB We have an extension to use a function for a type or we can do it explicitly if we want more context
            ComparerConvention<double>(AbsDouble);
            ComparerConvention<double>(x => (x == typeof(double)), AbsDouble);

            // Pick up all explicitly defined checkers.
            Register(typeof(CheckerFactory).Assembly);
            Register(typeof(NCheck.CheckerFactory).Assembly);
        }

        public bool AbsDouble(double x, double y)
        {
            return Math.Abs(x - y) < 0.001;
        }

        public bool AbsFloat(float x, float y)
        {
            return Math.Abs(x - y) < 0.001;
        }
    }
```

These conventions apply when using the automatic checker creation or when using Initialize inside a custom Checker, and any changes to the default should be registered inside
CheckerFactory.Initialize.

```csharp
   ...
        [Test]
        public void CheckerFactoryRegisterTypeViaGeneric()
        {
            var cf = new CheckerFactory();
            cf.Convention<SampleClass>(CompareTarget.Ignore);

            Assert.AreEqual(CompareTarget.Ignore, PropertyCheck.TypeConventions.CompareTarget.Convention(typeof(SampleClass)));
        }

        [Test]
        public void DetermineValueBasedOnName()
        {
            var targeter = new PropertyConventions();
            targeter.CompareTarget.Register(x => x.Name == "Ignore", CompareTarget.Ignore);

            CheckTargetType<SampleClass>(targeter, x => x.Ignore, CompareTarget.Ignore);  
        }
    ...
```

### Customizing a checker

If you can't achieve your required behaviour with convention, you will need to define a custom checker, example below...

```csharp
    public class SimpleChecker : Checker<Simple>
    {
        public SimpleChecker()
        {
            Compare(x => x.Id);
            Compare(x => x.Name);
            Compare(x => x.Value).Value<double>((x, y) => Math.Abs(x - y) < 0.001);
        }
    }
```

This fully defines the behaviour of the Checker, including the use of a custom comparer to limit the precision of comparison for the double values.

You can also initialize the Checker with the default behaviour, and then override it as necessary as follows...

```csharp
    public class SimpleChecker : Checker<Simple>
    {
        public SimpleChecker()
        {
            Initialize();
            Compare(x => x.Id).Ignore;
        }
    }
```

### Managing Object Identity

For complex object graphs it may not be necessary, or useful (think about cyclic references) to compare all properties, but to just ensure than the identity of the expected and candidate entities are the same. To this end the library supports the idea of an identity checker via IIdentityChecker which allows the developer to implement a domain-specific checker.

Say we have an interface in our domain model IIdentifiable as follows, with a sample usage

```csharp
    public interface IIdentifiable
    {
        int Id { get; set; }
    }

    public class Customer : IIdentifiable
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
```

We can write an implementation of IIdentityChecker which limits to just checking the identity of the object instances, this is useful if the database is involved and values
such as audit information may have been updated.

```csharp
    public class IdentifiableChecker : IIdentityChecker
    {
        public bool SupportsId(Type type)
        {
            return typeof(IIdentifiable).IsAssignableFrom(type);
        }

        public object ExtractId(object value)
        {
            var x = value as IIdentifiable;
            return x == null ? null : x.Id;
        }
    }
```

You then register and instance of this class in your custom CheckerFactory
```csharp
    ...
        public CheckerFactory()
        {
            PropertyCheck.IdentityChecker = new IdentifiableChecker();

            Initialize();
        }
    ...
```

This can then be easily used to break object cycles as follows...

```csharp
    public interface IIdentifiable
    {
        int Id { get; set; }
    }

    public class Order : IIdentifiable
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public IList<OrderLine> { get; set; }
        
        ....
    }
    
    public class OrderLine : IIdentifiable
    {
        public int Id { get; set; }
        
        public Order Order { get; set; }
        
        ....
    }
    
    public class OrderLineChecker : Checker<OrderLine>
    {
        public OrderLineChecker()
        {
            Initialize();
            Compare(x => x.Order).Id;
        }
    }
```


### Allowing for failure

For negative testing, you might need to prove that the comparison fails in a particular manner, we allow for this with a couple of overloads...

```csharp
    [TestFixture]
    public class SimpleTest
    {
        [Test]
        public void AlgoFailTest()
        {
            var checkerFactory = new CheckerFactory();

            var algo = new ShinyBusinessService();
            var source = new Simple { Id = 1, Name = "A", Value = 1.0 } ;

            var expected = new Simple { Id = 2, Name = "B", Value = 1.0 } ;

            var candidate = algo.Run(source);

            CheckFault(expected, candidate, "Simple.Value", 1.0, 1.2);
        }
    }
```

This will check for a specific failure in the comparsion, the other overload allows you to provide the exact message rather than letting the library format it.

### Per-Test customization

For specific tests, you might want to override the standard Checker for the class, be it automatically constructed or one you have explicitly defined.

To do this, ICheckerFactory exposes the Compare<T> interface used to specify property comparisons, here are some examples taken from the unit tests; the Parent checker
has been defined to specifically ignore the Another property.
```csharp
    ...
    public class ParentChecker : Checker<Parent>
    {
        public ParentChecker()
        {
            Initialize();
            Compare(x => x.Another).Ignore();
        }
    }
    ...
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
    ...
```

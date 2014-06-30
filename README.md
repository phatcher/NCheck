NCheck
======

Library providing object checking features primarily used for state based testing.

Available as a NuGet package [NCheck](https://www.nuget.org/packages/NCheck/)

Welcome to contributions from anyone.

You can see the version history [here](RELEASE_NOTES.md).

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
            var algo = new ShinyBusinssFunction();
            var source = new Simple { Id = 1, Name = "A", Value = 1.0 } ;

            var candidate = algo.Run();
            Assert.AreEqual(2, candidate.Id, "Id differs");
            Assert.AreEqual("B", candidate.Name, "Name differs");
            Assert.AreEqual(1.2, candidate.Value, "Value differs");
        }
    }
```
The problem arises when things change, say we add two more properties to Simple and these are affected by our ShinyBusinessFunction.

Unfortunately, our test still passes since it does not know about the new properties and so ignores them, so we will typically pass our unit test phase and will only find out at integration testing that we have forgotten to take account of the new state.

Our solution is too introduce some helper classes and methods into the tests which will catch this type of error during unit testing (much cheaper!).

If we re-write the test using the NCheck library we get the following
```csharp
    [TestFixture]
    public class SimpleTest
    {
        [Test]
        public void AlgoTest()
        {
            var checkerFactory = new CheckerFactory();

            var algo = new ShinyBusinssFunction();
            var source = new Simple { Id = 1, Name = "A", Value = 1.0 } ;

            var expected = new Simple { Id = 2, Name = "B", Value = 1.2 } ;

            var candidate = algo.Run();
            checkerFactory.Check(expected, candidate);
        }
    }
```
Now when the definition of Simple and the algorithm changes, this test will fail because the expected value will not match since it does not define values for the new properties.

Couple of things to note here...
1. We have gone from comparing individual properties to comparing objects
1. By default we don't need to create a checker for an object, the library does this for us automatically
1. You typically subclass CheckerFactory and put a property in your test fixtures to create it on first use.



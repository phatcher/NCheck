namespace NCheck
{
    using System;

    /// <summary>
    /// Use reflection to construct a checker for a type
    /// </summary>
    public class CheckerBuilder : ICheckerBuilder
    {
        /// <summary>
        /// Create a new instance of the <see cref="CheckerBuilder" /> class.
        /// </summary>
        /// <param name="factory"></param>
        public CheckerBuilder(ICheckerFactory factory)
        {
            // Set this as the global factory, needed by individual checkers if they do Entity checks
            Checker.CheckerFactory = factory;
        }

        /// <summary>
        /// Build a <see cref="IChecker"/> for a type.
        /// </summary>
        /// <param name="type">Type to use</param>
        /// <returns>A <see cref="Checker{T}" /> for the type.</returns>
        public IChecker Build(Type type)
        {
            var genericType = typeof(Checker<>);

            var checkerType = genericType.MakeGenericType(type);

            var checker = (IChecker)Activator.CreateInstance(checkerType);
            var initializer = (ICheckerInitializer)checker;

            // Standard set of comparers
            initializer.AutoCheck(type);

            return checker;
        }
    }
}
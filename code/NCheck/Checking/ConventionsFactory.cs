using System;

namespace NCheck.Checking
{
    /// <summary>
    /// Factory for <see cref="CheckerConventions"/>.
    /// </summary>
    public static class ConventionsFactory
    {
        private static CheckerConventions conventions;

        static ConventionsFactory()
        {
            IdentityCheckerFactory = () => new NullIdentityChecker();
            TypeConventionsFactory = c => c.TypeConventions.InitializeTypeConventions();
        }

        /// <summary>
        /// Get or set the type that initialized the factory.
        /// <para>
        /// Can be used to determine if we need to register the factory functions.
        /// </para>
        /// </summary>
        public static Type FactoryType { get; set; }

        /// <summary>
        /// Get or set the identity checker factory (default: NullIdentityChecker)
        /// </summary>
        public static Func<IIdentityChecker> IdentityCheckerFactory { get; set; }

        /// <summary>
        /// Get or set the property checker factory (default: null)
        /// </summary>
        public static Action<CheckerConventions> PropertyConventionsFactory { get; set; }

        /// <summary>
        /// Get or set the type convention factory (default: InitializeTypeConventions)
        /// </summary>
        public static Action<CheckerConventions> TypeConventionsFactory { get; set; }

        /// <summary>
        /// Get or set the comparer convention factory (default: null)
        /// </summary>
        public static Action<CheckerConventions> ComparerConventionsFactory { get; set; }

        /// <summary>
        /// Gets or sets the class which knows the conventions.
        /// </summary>
        public static CheckerConventions Conventions
        {
            get
            {
                if (conventions == null)
                {
                    // Create it
                    conventions = new CheckerConventions();

                    // Now apply the various factories
                    if (IdentityCheckerFactory != null)
                    {
                        conventions.IdentityChecker = IdentityCheckerFactory();
                    }

                    PropertyConventionsFactory?.Invoke(conventions);
                    TypeConventionsFactory?.Invoke(conventions);
                    ComparerConventionsFactory?.Invoke(conventions);
                }

                return conventions;
            }
            set => conventions = value;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using NCheck.Checking;

namespace NCheck
{
    /// <summary>
    /// A factory class that provides checking facilities for objects so that property level comparisons can be easily made
    /// </summary>
    public class CheckerFactory : ICheckerFactory
    {        
        private readonly IDictionary<Type, IChecker> checkers;
        private readonly object syncLock;
        private CheckerConventions conventions;
        private ICheckerBuilder builder;

        /// <summary>
        /// Initializes a new instance of the CheckerClassFactory class.
        /// </summary>
        public CheckerFactory()
        {
            checkers = new Dictionary<Type, IChecker>();
            syncLock = new object();

            // Set us up as the global factory, used to locate the checkers later on.
            lock (syncLock)
            {
                NCheck.Checker.CheckerFactory = this;
            }
        }

        /// <summary>
        /// Gets or sets the checker builder
        /// </summary>
        public ICheckerBuilder Builder
        {
            get => builder ?? (builder = new CheckerBuilder(this));
            set => builder = value;
        }

        /// <summary>
        /// Get or sets the conventions
        /// </summary>
        public CheckerConventions Conventions
        {
            get => conventions ?? (conventions = ConventionsFactory.Conventions);
            set => conventions = value;
        }

        /// <summary>
        /// Clear the registered checkers
        /// </summary>
        public void Clear()
        {
            checkers.Clear();
        }

        /// <summary>
        /// General entry point that works out which checker to invoke.
        /// </summary>
        /// <param name="expected">Object containing expected values</param>
        /// <param name="candidate">Object containing values to check</param>
        /// <param name="objectName">Name of the object, used to disambiguate error messages</param>
        /// <remarks>
        /// We use the expected to determine the type of the checker.
        /// </remarks>
        public void Check(object expected, object candidate, string objectName = "")
        {
            if (expected == null && candidate == null)
            {
                return;
            }

            var type = expected != null ? expected.GetType() : candidate.GetType();
            Checker(type).Check(expected, candidate, objectName);
        }

        /// <copydocfrom cref="ICheckerFactory.Check{T}(IEnumerable{T}, IEnumerable{T}, string)" />
        public void Check<T>(T expected, T candidate, string objectName = "")
        {
            if (Verify(candidate, expected, objectName))
            {
                Checker<T>().Check(expected, candidate, objectName);
            }
        }

        /// <copydocfrom cref="ICheckerFactory.Check{T}(IEnumerable{T}, IEnumerable{T}, string)" />
        public void Check<T>(IEnumerable<T> expectedList, IEnumerable<T> candidateList, string objectName = "")
        {
            var checker = Checker<T>();
            var listChecker = new ListChecker();

            // NB Can't call Check as this retrieves via the property info which we don't have here.
            listChecker.CheckList(checker as IChecker, expectedList, candidateList, objectName);
        }

        /// <copydocfrom cref="ICheckerFactory.CheckParent{T}" />
        public void CheckParent<T>(T expected, T candidate, string objectName = "")
        {
            if (Verify(candidate, expected, objectName))
            {
                Checker<T>().CheckBase(expected, candidate, objectName);
            }
        }

        /// <copydocfrom cref="ICheckerFactory.Compare{T}" />
        public PropertyCheckExpression Compare<T>(Expression<Func<T, object>> propertyExpression)
        {
            // Get the checker for the entity
            var checker = (ICheckerInitializer) Checker<T>();

            // Expose the property checker expression for the property.
            return checker.Compare(propertyExpression.GetPropertyInfo());
        }

        /// <summary>
        /// Registers all <see cref="IChecker{T}" /> implemented in an assembly
        /// </summary>
        /// <param name="assemblyFile">Filename of the assembly to register</param>
        public CheckerFactory Register(string assemblyFile)
        {
            if (!assemblyFile.EndsWith(".dll"))
            {
                assemblyFile += ".dll";
            }

#if !NETSTANDARD
            Register(Assembly.LoadFrom(assemblyFile));
#else
            throw new NotSupportedException("Cannot load assemblies");
            //Register(Assembly.LoadFromAssemblyPath(assemblyFile));
#endif

            return this;
        }

        /// <summary>
        /// Registers all <see cref="IChecker{T}" /> implemented in an assembly
        /// </summary>
        /// <param name="assembly">Assembly to register</param>
        public CheckerFactory Register(Assembly assembly)
        {
            lock (syncLock)
            {
                foreach (var type in assembly.GetTypes())
                {
                    Register(type);
                }
            }

            return this;
        }

        /// <summary>
        /// Register a <see cref="IChecker{T}" /> for <see typeparamref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="checker"></param>
        public CheckerFactory Register<T>(IChecker<T> checker)
        {
            Register(typeof(T), (IChecker)checker);

            return this;
        }


        private void Register(Type type)
        {
            try
            {
#if !NETSTANDARD
                if (!SupportsGenericInterface(type, typeof(IChecker<>)) || type.ContainsGenericParameters)
#else
                if (!SupportsGenericInterface(type, typeof(IChecker<>)) || type.GetTypeInfo().ContainsGenericParameters)
#endif
                {
                    return;
                }

                var checker = Activator.CreateInstance(type);
#if !NETSTANDARD
                var fred = type.BaseType;

                while (!fred.IsGenericType)
                {
                    fred = fred.BaseType;
                }

                var entityType = fred.GetGenericArguments();
#else
                var fred = type.GetTypeInfo().BaseType;
                while (!fred.GetTypeInfo().IsGenericType)
                {
                    fred = fred.GetTypeInfo().BaseType;
                }

                var entityType = fred.GetTypeInfo().GetGenericArguments();
#endif
                Register(entityType[0], (IChecker)checker);

            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Could not register " + type.Name, ex);
            }
        }

        /// <summary>
        /// Check if a type supports a generic interface
        /// </summary>
        /// <param name="type"></param>
        /// <param name="candidate"></param>
        /// <returns></returns>
        private static bool SupportsGenericInterface(Type type, Type candidate)
        {
            if (candidate == null)
            {
                throw new ArgumentNullException(nameof(candidate));
            }

#if !NETSTANDARD
            if (candidate.IsGenericType == false)
#else
            if (candidate.GetTypeInfo().IsGenericType == false)
#endif
            {
                throw new ArgumentOutOfRangeException(nameof(candidate), "Must be a generic type");
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

#if !NETSTANDARD
            return type.GetInterfaces().Where(i => i.IsGenericType).Any(i => candidate == i.GetGenericTypeDefinition());
#else
            return type.GetTypeInfo().GetInterfaces().Where(i => i.GetTypeInfo().IsGenericType).Any(i => candidate == i.GetGenericTypeDefinition());
#endif
        }

        private static bool Verify<T>(T candidate, T expected, string objectName)
        {
            var type = typeof(T);
#if !NETSTANDARD
            if (!type.IsValueType)
#else
            if (!type.GetTypeInfo().IsValueType)
#endif
            {
                if (candidate == null && expected == null)
                {
                    return false;
                }

                if (candidate == null)
                {
                    throw new PropertyCheckException(objectName ?? type.Name, "not null", "null");
                }

                if (expected == null)
                {
                    throw new PropertyCheckException(objectName ?? type.Name, "null", "not null");
                }
            }

            return true;
        }

        private IChecker<T> Checker<T>()
        {
            return (IChecker<T>)Checker(typeof(T));
        }

        private IChecker Checker(Type type)
        {
            if (checkers.TryGetValue(type, out var checker))
            {
                return checker;
            }

            lock (syncLock)
            {
                if (checkers.TryGetValue(type, out checker))
                {
                    return checker;
                }

                // Attempt to self-register
                checker = Builder.Build(type);
                if (checker != null)
                {
                    Register(type, checker);

                    return checker;
                }

                throw new NotSupportedException($"No checker registered for {type.FullName}");
            }
        }

        private void Register(Type entityType, IChecker checker)
        {
            lock (syncLock)
            {
                // Use assignment so we can override an existing checker.
                checkers[entityType] = checker;
            }
        }

        private class ListChecker : PropertyCheck
        {
            public ListChecker() : base(null, CompareTarget.Collection)
            {
            }

            public void CheckList(IChecker checker, IEnumerable expected, IEnumerable candidate, string objectName)
            {
                Check(checker, expected, candidate, objectName);
            }
        }
    }
}
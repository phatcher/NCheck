namespace NCheck
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    using NCheck.Checking;

    /// <summary>
    /// Extension methods for the checkers.
    /// </summary>
    public static class CheckerExtensions
    {
        /// <summary>
        /// Add all public properties to a comparer.
        /// </summary>
        /// <param name="compare">Comparer to use</param>
        /// <param name="type">Type to use</param>
        public static void AutoCheck(this ICheckerCompare compare, Type type)
        {
            // Only get directly declared properties - parent will have own checker
            var properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            compare.AutoCheck(properties);
        }

        /// <summary>
        /// Add properties to a comparer.
        /// </summary>
        /// <param name="compare">Comparer to use</param>
        /// <param name="properties">Properties to add</param>
        public static void AutoCheck(this ICheckerCompare compare, IEnumerable<PropertyInfo> properties)
        {
            // Setup the comparisons
            foreach (var prop in properties)
            {
                // Exclude indexers
                var x = prop.GetIndexParameters();
                if (x.GetLength(0) != 0)
                {
                    continue;
                }

                compare.Compare(prop);
            }
        }

        /// <summary>
        /// Sets the default behaviour for a <see cref="ITypeCompareTargeter"/>
        /// </summary>
        /// <param name="targeter"></param>
        public static void InitializeTypeCompareTargeter(this ITypeCompareTargeter targeter)
        {
            targeter.Register(ReferenceTypeAsEntity);
            targeter.Register(NonNullableStructsAsEntity);
            targeter.Register(EnumerableAsCollection);
            targeter.Register(IsEnum);

            // Put the default overrides in
            targeter.Register(typeof(Guid), CompareTarget.Value);
            targeter.Register(typeof(String), CompareTarget.Value);
            targeter.Register(typeof(Decimal), CompareTarget.Value);
            targeter.Register(typeof(Single), CompareTarget.Value);
            targeter.Register(typeof(Double), CompareTarget.Value);
            targeter.Register(typeof(DateTime), CompareTarget.Value);
            targeter.Register(typeof(DateTimeOffset), CompareTarget.Value);
            targeter.Register(typeof(TimeSpan), CompareTarget.Value);
            targeter.Register(typeof(TimeZone), CompareTarget.Value);
            targeter.Register(typeof(TimeZoneInfo), CompareTarget.Value);

            // Type is abstract, is RuntimeType at r/t which is internal so can't handle easily
            targeter.Register(typeof(Type), CompareTarget.Value);            
        }

        private static CompareTarget IsEnum(Type type)
        {
            return type.IsEnum ? CompareTarget.Value : CompareTarget.Unknown;
        }

        private static CompareTarget NonNullableStructsAsEntity(Type type)
        {
            if (!type.IsValueType)
            {
                return CompareTarget.Unknown;
            }

            // Trying to make sure we process structs as entities, but exclude nullables
            if (!type.IsPrimitive && !type.FullName.StartsWith("System.Nullable"))
            {
                return CompareTarget.Entity;
            }

            return CompareTarget.Unknown;
        }

        private static CompareTarget ReferenceTypeAsEntity(Type type)
        {
            return !type.IsValueType ? CompareTarget.Entity : CompareTarget.Unknown;
        }

        private static CompareTarget EnumerableAsCollection(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) ? CompareTarget.Collection : CompareTarget.Unknown;
        }
    }
}
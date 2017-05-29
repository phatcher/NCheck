using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using NCheck.Checking;

namespace NCheck
{
    /// <summary>
    /// Extension methods for the checkers.
    /// </summary>
    public static class CheckerExtensions
    {
        /// <summary>
        /// Add all properties to a comparer.
        /// </summary>
        /// <param name="compare">Comparer to use</param>
        /// <param name="type">Type to use</param>
        /// <param name="flags">Binding flags to use, default to Public properties</param>
        public static void AutoCheck(this ICheckerCompare compare, Type type, BindingFlags flags = BindingFlags.Public)
        {
            // Only get directly declared properties - parent will have own checker
            var properties = type.GetProperties(flags | BindingFlags.DeclaredOnly | BindingFlags.Instance);

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
        /// Sets the default behaviour for type <see cref="CompareTarget"/> conventions e.g. Enum -> Value
        /// </summary>
        /// <param name="conventions"></param>
        public static void InitializeTypeConventions(this IConventions<Type> conventions)
        {
            conventions.CompareTarget.InitializeTypeCompareTargetConventions();
        }

        /// <summary>
        /// Sets the default behaviour for a <see cref="IConventions{Type, CompareTarget}"/>  e.g. Enum -> Value
        /// </summary>
        /// <param name="conventions"></param>
        public static void InitializeTypeCompareTargetConventions(this IConventions<Type, CompareTarget> conventions)
        {
            conventions.Register(ReferenceTypeAsEntity, CompareTarget.Entity);
            conventions.Register(NonNullableStructsAsEntity, CompareTarget.Entity);
            conventions.Register(EnumerableAsCollection, CompareTarget.Collection);
            // NB Must be before Enumerable
            conventions.Register(DictionaryConvention, CompareTarget.Dictionary);
            conventions.Register(IsEnum, CompareTarget.Value);

            // Put the default overrides in
            conventions.Register(typeof(Guid), CompareTarget.Value);
            conventions.Register(typeof(string), CompareTarget.Value);
            conventions.Register(typeof(decimal), CompareTarget.Value);
            conventions.Register(typeof(float), CompareTarget.Value);
            conventions.Register(typeof(double), CompareTarget.Value);
            conventions.Register(typeof(DateTime), CompareTarget.Value);
            conventions.Register(typeof(DateTimeOffset), CompareTarget.Value);
            conventions.Register(typeof(TimeSpan), CompareTarget.Value);
            // TODO: Conditional registration if .NET or NetStandard > 2.0
            conventions.Register(typeof(TimeZone), CompareTarget.Value);
            conventions.Register(typeof(TimeZoneInfo), CompareTarget.Value);

            // Type is abstract, is RuntimeType at r/t which is internal so can't handle easily
            conventions.Register(typeof(Type), CompareTarget.Value);      
        }

        private static bool IsEnum(Type type)
        {
            return type.IsEnum;
        }

        private static bool NonNullableStructsAsEntity(Type type)
        {
            if (!type.IsValueType)
            {
                return false;
            }

            // Trying to make sure we process structs as entities, but exclude nullables
            if (!type.IsPrimitive && !type.FullName.StartsWith("System.Nullable"))
            {
                return true;
            }

            return false;
        }

        private static bool ReferenceTypeAsEntity(Type type)
        {
            return !type.IsValueType;
        }

        private static bool DictionaryConvention(Type type)
        {
            return typeof(IDictionary).IsAssignableFrom(type) || (type.IsGenericType && typeof(IDictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition()));
        }

        private static bool EnumerableAsCollection(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }
    }
}
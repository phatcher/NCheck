namespace NCheck
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

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
    }
}
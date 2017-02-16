using System;
using System.Reflection;

namespace NCheck.Checking
{
    /// <summary>
    /// Extension methods for registering <see cref="IConventions{T}"/>
    /// </summary>
    public static class ConventionExtensions
    {
        /// <summary>
        /// Register a <see cref="CompareTarget"/> convention for a type.
        /// </summary>
        /// <typeparam name="T">Type to use</typeparam>
        /// <param name="conventions"></param>
        /// <param name="target">CompareTarget value to return</param>
        public static void Convention<T>(this IConventions<Type> conventions, CompareTarget target)
        {
            conventions.Convention(typeof(T), target);
        }

        /// <summary>
        /// Register a <see cref="CompareTarget"/> convention for a type.
        /// </summary>
        /// <param name="conventions"></param>
        /// <param name="type">Type to use</param>
        /// <param name="target">CompareTarget value to return</param>
        public static void Convention(this IConventions<Type> conventions, Type type, CompareTarget target)
        {
            conventions.CompareTarget.Register(type, target);
        }

        /// <summary>
        /// Register an explicit <see cref="CompareTarget"/> to use for a type.
        /// </summary>
        /// <param name="conventions">Comparer to register against</param>
        /// <param name="type">Type to use</param>
        /// <param name="target">CompareTarget to use</param>
        public static void Register(this IConventions<Type, CompareTarget> conventions, Type type, CompareTarget target)
        {
            conventions.Register(x => x.FullName == type.FullName, target);
        }

        /// <summary>
        /// Register a <see cref="CompareTarget"/> convention based on type information.
        /// </summary>
        /// <param name="conventions"></param>
        /// <param name="func"></param>
        /// <param name="value"></param>
        public static void Convention(this IConventions<Type> conventions, Func<Type, bool> func, CompareTarget value)
        {
            conventions.CompareTarget.Register(func, value);
        }

        /// <summary>
        /// Register a <see cref="CompareTarget"/> convention based on property information.
        /// </summary>
        /// <param name="conventions"></param>
        /// <param name="func"></param>
        /// <param name="value"></param>
        public static void Convention(this IConventions<PropertyInfo> conventions, Func<PropertyInfo, bool> func, CompareTarget value)
        {
            conventions.CompareTarget.Register(func, value);
        }

        /// <summary>
        /// Register an equality comparer convention based on type information.
        /// </summary>
        /// <typeparam name="T">Type to use</typeparam>
        /// <param name="conventions"></param>
        /// <param name="value">Equality function to apply</param>
        public static void ComparerConvention<T>(this IConventions<Type> conventions, Func<T, T, bool> value)
        {
            conventions.ComparerConvention(x => (x == typeof(T)), value.ToComparerConvention());
        }

        /// <summary>
        /// Register an equality comparer convention based on type information.
        /// </summary>
        /// <param name="conventions"></param>
        /// <param name="typeChecker"></param>
        /// <param name="func"></param>
        public static void ComparerConvention<T>(this IConventions<Type> conventions, Func<Type, bool> typeChecker, Func<T, T, bool> func)
        {
            conventions.ComparerConvention(typeChecker, func.ToComparerConvention());
        }

        /// <summary>
        /// Register an equality comparer convention based on type information.
        /// </summary>
        /// <param name="conventions"></param>
        /// <param name="typeChecker"></param>
        /// <param name="func"></param>
        public static void ComparerConvention(this IConventions<Type> conventions, Func<Type, bool> typeChecker, Func<object, object, bool> func)
        {
            conventions.Comparer.Register(typeChecker, func);
        }

        /// <summary>
        /// Register an equality comparer convention based on property information.
        /// </summary>
        /// <param name="conventions"></param>
        /// <param name="func"></param>
        /// <param name="value"></param>
        public static void ComparerConvention(this IConventions<PropertyInfo> conventions, Func<PropertyInfo, bool> func, Func<object, object, bool> value)
        {
            conventions.Comparer.Register(func, value);
        }

        /// <summary>
        /// Converts a strongly typed function into a comparer convention
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        public static Func<object, object, bool> ToComparerConvention<T>(this Func<T, T, bool> func)
        {
            // NB The cast could fail at runtime and this will also cause boxing.
            return (x, y) => func((T)x, (T)y);
        }
    }
}

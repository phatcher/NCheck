using System;

namespace NCheck.Checking
{
    /// <summary>
    /// Conventions used in checking.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface IConventions<TSource>
    {
        /// <summary>
        /// Clears the conventions.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the comparer conventions.
        /// </summary>
        IConventions<TSource, Func<object, object, bool>> Comparer { get; }

        /// <summary>
        /// Gets the <see cref="CompareTarget"/> conventions.
        /// </summary>
        IConventions<TSource, CompareTarget> CompareTarget { get; }
    }
}
using System;

namespace NCheck.Checking
{
    /// <summary>
    /// Determine the convention from source to target based on a comparer function based and a source
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    public interface IConventions<TSource, TTarget>
    {
        /// <summary>
        /// Gets the target value if no convention holds
        /// </summary>
        TTarget DefaultConvention { get; }

        /// <summary>
        /// Clears the conventions
        /// </summary>
        void Clear();

        /// <summary>
        /// Get the convention based on the source.
        /// </summary>
        /// <param name="source">Source to use</param>
        /// <returns>Returns the target value of a convention or <see cref="DefaultConvention"/> if no convention applies</returns>
        TTarget Convention(TSource source);

        /// <summary>
        /// Register a convention
        /// </summary>
        /// <param name="func">Function to determine if the convention applies</param>
        /// <param name="value">Value to return if convention applies</param>
        /// <remarks>No checking for duplicates is performed and last registration wins in case of two competing conventions</remarks>
        void Register(Func<TSource, bool> func, TTarget value);
    }
}
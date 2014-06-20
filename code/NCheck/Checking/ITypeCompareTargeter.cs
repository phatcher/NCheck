namespace NCheck.Checking
{
    using System;

    /// <summary>
    /// Determine the type of <see cref="CompareTarget"/> to use based on the type.
    /// </summary>
    public interface ITypeCompareTargeter
    {
        /// <summary>
        /// Clears al registrations.
        /// </summary>
        void Clear();

        /// <summary>
        /// Determine the type of <see cref="CompareTarget"/> to use based on the type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        CompareTarget DetermineCompareTarget(Type type);

        /// <summary>
        /// Register an explicit <see cref="CompareTarget"/> to use for a type.
        /// </summary>
        /// <param name="type">Type to use</param>
        /// <param name="target">CompareTarget to use</param>
        void Register(Type type, CompareTarget target);

        /// <summary>
        /// Registers a function to determine the <see cref="CompareTarget"/> for a type.
        /// </summary>
        /// <param name="func">Function to use.</param>
        void Register(Func<Type, CompareTarget> func);
    }
}
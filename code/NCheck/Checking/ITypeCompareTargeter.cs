namespace NCheck.Checking
{
    using System;

    /// <summary>
    /// Determine the type of <see cref="CompareTarget"/> to use based on the type.
    /// </summary>
    public interface ITypeCompareTargeter
    {
        /// <summary>
        /// Determine the type of <see cref="CompareTarget"/> to use based on the type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        CompareTarget DetermineCompareTarget(Type type);

        /// <summary>
        /// Allows registration of an explict <see cref="CompareTarget"/> overriding the default behaviour.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        void Register(Type type, CompareTarget target);
    }
}
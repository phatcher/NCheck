namespace NCheck.Checking
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Determine the type of <see cref="CompareTarget"/> to use based on the property.
    /// </summary>
    public interface IPropertyCompareTargeter
    {
        /// <summary>
        /// Determine the type of <see cref="CompareTarget"/> to use based on a property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        CompareTarget DetermineCompareTarget(PropertyInfo property);

        /// <summary>
        /// Registers a function to determine the <see cref="CompareTarget"/>
        /// </summary>
        /// <param name="func"></param>
        void Register(Func<PropertyInfo, CompareTarget> func);
    }
}

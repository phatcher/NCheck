﻿namespace NCheck
{
    using System.Collections.Generic;
    using System.Reflection;

    using NCheck.Checking;

    /// <summary>
    /// Initialization methods for a checker
    /// </summary>
    public interface ICheckerInitializer
    {
        /// <summary>
        /// Collection of <see cref="PropertyCheck"/>s to use.
        /// </summary>
        ICollection<PropertyCheck> Properties { get; }

        /// <summary>
        /// Property to include in the collection.
        /// </summary>
        /// <param name="propertyInfo">PropertyInfo to use</param>
        /// <returns>A new <see cref="PropertyCheckExpression" /> created from the <see cref="PropertyInfo" /></returns>
        PropertyCheckExpression Compare(PropertyInfo propertyInfo);
    }
}
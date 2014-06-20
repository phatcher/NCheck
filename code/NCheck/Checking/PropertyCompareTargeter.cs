namespace NCheck.Checking
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <copydocfrom cref="IPropertyCompareTargeter" />
    public class PropertyCompareTargeter : IPropertyCompareTargeter
    {
        private readonly List<Func<PropertyInfo, CompareTarget>> targeters;

        /// <summary>
        /// Creates a new instance of the <see cref="PropertyCompareTargeter" /> class.
        /// </summary>
        public PropertyCompareTargeter()
        {
            targeters = new List<Func<PropertyInfo, CompareTarget>>();
        }

        /// <copydocfrom cref="IPropertyCompareTargeter.DetermineCompareTarget" />
        public CompareTarget DetermineCompareTarget(PropertyInfo property)
        {
            foreach (var func in targeters)
            {
                var result = func(property);
                if (result != CompareTarget.Unknown)
                {
                    return result;
                }
            }

            return CompareTarget.Unknown;
        }

        /// <copydocfrom cref="IPropertyCompareTargeter.Register" />
        public void Register(Func<PropertyInfo, CompareTarget> func)
        {
            targeters.Add(func);
        }
    }
}

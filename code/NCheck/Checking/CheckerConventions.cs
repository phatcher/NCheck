using System;
using System.Reflection;

namespace NCheck.Checking
{
    /// <summary>
    /// Provides type and property conventions to the checkers.
    /// </summary>
    public class CheckerConventions
    {
        /// <summary>
        /// Creates a new instance of the <see cref="CheckerConventions"/> class.
        /// </summary>
        public CheckerConventions()
        {
            Clear();
        }

        /// <summary>
        /// Gets or sets the class which knows how to extract an Id from an object
        /// </summary>
        public IIdentityChecker IdentityChecker { get; set; }

        /// <summary>
        /// Gets or sets the class which knows the default <see cref="CompareTarget"/> for a property.
        /// <para>
        /// This allows the introduction of conventions based on property names.
        /// </para>
        /// </summary>
        public IConventions<PropertyInfo> PropertyConventions { get; set; }

        /// <summary>
        /// Gets or sets the class which knows the conventions for a type.
        /// </summary>
        public IConventions<Type> TypeConventions { get; set; }

        /// <summary>
        /// Clear any conventions
        /// </summary>
        public void Clear()
        {
            IdentityChecker = new NullIdentityChecker();
            TypeConventions = new TypeConventions();
            PropertyConventions = new PropertyConventions();
        }

        /// <summary>
        /// Register a <see cref="CompareTarget"/> convention for a type.
        /// </summary>
        /// <typeparam name="T">Type to use</typeparam>
        /// <param name="target">CompareTarget value to return</param>
        public void Convention<T>(CompareTarget target)
        {
            TypeConventions.Convention<T>(target);
        }

        /// <summary>
        /// Register a <see cref="CompareTarget"/> convention for a type.
        /// </summary>
        /// <param name="type">Type to use</param>
        /// <param name="target">CompareTarget value to return</param>
        public void Convention(Type type, CompareTarget target)
        {
            TypeConventions.CompareTarget.Register(type, target);
        }

        /// <summary>
        /// Register a <see cref="CompareTarget"/> convention based on type information.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="value"></param>
        public void Convention(Func<Type, bool> func, CompareTarget value)
        {
            TypeConventions.Convention(func, value);
        }

        /// <summary>
        /// Register an equality comparer convention based on property information.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="value"></param>
        public void Convention(Func<PropertyInfo, bool> func, CompareTarget value)
        {
            PropertyConventions.CompareTarget.Register(func, value);
        }

        /// <summary>
        /// Determine the comparison target for a property.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public CompareTarget CompareTarget(PropertyInfo propertyInfo)
        {
            CompareTarget target;

            if (PropertyConventions != null)
            {
                target = PropertyConventions.CompareTarget.Convention(propertyInfo);
                if (target != Checking.CompareTarget.Unknown)
                {
                    return target;
                }
            }

            if (TypeConventions == null)
            {
                throw new NotSupportedException("No type conventions assigned to CheckerConventions");
            }

            target = TypeConventions.CompareTarget.Convention(propertyInfo.PropertyType);

            return target != Checking.CompareTarget.Unknown ? target : Checking.CompareTarget.Value;
        }

        /// <summary>
        /// Register an equality comparer convention based on type information.
        /// </summary>
        /// <typeparam name="T">Type to use</typeparam>
        /// <param name="value">Equality function to apply</param>
        public void ComparerConvention<T>(Func<T, T, bool> value)
        {
            TypeConventions.ComparerConvention(value);
        }

        /// <summary>
        /// Register an equality comparer convention based on type information.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="value"></param>
        public void ComparerConvention<T>(Func<Type, bool> func, Func<T, T, bool> value)
        {
            TypeConventions.ComparerConvention(func, value);
        }

        /// <summary>
        /// Register an equality comparer convention based on type information.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="value"></param>
        public void ComparerConvention(Func<Type, bool> func, Func<object, object, bool> value)
        {
            TypeConventions.ComparerConvention(func, value);
        }

        /// <summary>
        /// Register an equality comparer convention based on property information.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="value"></param>
        public void ComparerConvention(Func<PropertyInfo, bool> func, Func<object, object, bool> value)
        {
            PropertyConventions.ComparerConvention(func, value);
        }
    }
}

namespace NCheck.Checking
{
    using System;

    /// <summary>
    /// Fluent interface for <see cref="PropertyCheck" />
    /// </summary>
    public class PropertyCheckExpression
    {
        private readonly PropertyCheck propertyCheck;

        /// <summary>
        /// Create a new instance of the <see cref="PropertyCheckExpression" /> class.
        /// </summary>
        /// <param name="propertyCheck"></param>
        public PropertyCheckExpression(PropertyCheck propertyCheck)
        {
            this.propertyCheck = propertyCheck;
        }

        /// <summary>
        /// Get the <see cref="PropertyCheck" />
        /// </summary>
        public PropertyCheck PropertyCheck
        {
            get { return propertyCheck; }
        }

        /// <summary>
        /// Set the compare target to <see cref="CompareTarget.Id"/>
        /// </summary>
        /// <param name="comparer">Function to determine equality</param>
        /// <returns></returns>
        public PropertyCheckExpression Id<T>(Func<T, T, bool> comparer)
        {
            return Id(comparer.ToComparerConvention());
        }

        /// <summary>
        /// Set the compare target to <see cref="CompareTarget.Id"/>
        /// </summary>
        /// <param name="comparer">Function to determine equality, defaults to object.Equals</param>
        /// <returns></returns>
        public PropertyCheckExpression Id(Func<object, object, bool> comparer = null)
        {
            propertyCheck.CompareTarget = CompareTarget.Id;
            propertyCheck.Comparer = comparer;

            return this;
        }

        /// <summary>
        /// Set the compare target to <see cref="CompareTarget.Ignore"/>
        /// </summary>
        /// <returns></returns>
        public PropertyCheckExpression Ignore()
        {
            propertyCheck.CompareTarget = CompareTarget.Ignore;

            return this;
        }

        /// <summary>
        /// Set the compare target to <see cref="CompareTarget.Entity"/>
        /// </summary>
        /// <returns></returns>
        public PropertyCheckExpression Entity()
        {
            propertyCheck.CompareTarget = CompareTarget.Entity;

            return this;
        }

        /// <summary>
        /// Set the compare target to <see cref="CompareTarget.Count"/>
        /// </summary>
        /// <returns></returns>
        public PropertyCheckExpression Count()
        {
            propertyCheck.CompareTarget = CompareTarget.Count;

            return this;
        }

        /// <summary>
        /// Set the compare target to <see cref="CompareTarget.Value"/>
        /// </summary>
        /// <param name="comparer">Function to determine equality</param>        
        /// <returns></returns>
        public PropertyCheckExpression Value<T>(Func<T, T, bool> comparer)
        {
            return Value(comparer.ToComparerConvention());
        }

        /// <summary>
        /// Set the compare target to <see cref="CompareTarget.Value"/>
        /// </summary>
        /// <param name="comparer">Function to determine equality, defaults to object.Equals</param>        
        /// <returns></returns>
        public PropertyCheckExpression Value(Func<object, object, bool> comparer = null)
        {
            propertyCheck.CompareTarget = CompareTarget.Value;
            propertyCheck.Comparer = comparer;

            return this;
        }

        /// <summary>
        /// Sets the length to compare.
        /// </summary>
        /// <returns></returns>
        public PropertyCheckExpression Length(int value)
        {
            propertyCheck.Length = value;

            return this;
        }
    }
}
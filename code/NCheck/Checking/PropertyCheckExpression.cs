namespace NCheck.Checking
{
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
        /// <returns></returns>
        public PropertyCheckExpression Id()
        {
            propertyCheck.CompareTarget = CompareTarget.Id;

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
        /// <returns></returns>
        public PropertyCheckExpression Value()
        {
            propertyCheck.CompareTarget = CompareTarget.Value;

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
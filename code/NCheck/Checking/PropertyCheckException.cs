namespace NCheck.Checking
{
    using System;

    /// <summary>
    /// Exception raised when a property comparison fails.
    /// </summary>
    public class PropertyCheckException : Exception
    {
        private const string Format = "{0}: Expected:<{1}>. Actual:<{2}>";

        /// <summary>
        /// Create a new instace of the <see cref="PropertyCheckException" /> class.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public PropertyCheckException(string propertyName, object expected, object actual)
            : base(string.Format(Format, propertyName, expected, actual))
        {
            PropertyName = propertyName;
            ExpectedValue = expected;
            ActualValue = actual;
        }

        /// <summary>
        /// Gets the property name.
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Gets the expected value.
        /// </summary>
        public object ExpectedValue { get; private set; }

        /// <summary>
        /// Gets the actual value.
        /// </summary>
        public object ActualValue { get; private set; }
    }
}
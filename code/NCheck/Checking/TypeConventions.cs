using System;

namespace NCheck.Checking
{
    /// <summary>
    /// Implements type conventions.
    /// </summary>
    public class TypeConventions : Conventions<Type>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="TypeConventions"/> class.
        /// </summary>
        public TypeConventions() : base(Checking.CompareTarget.Value)
        {
        }
    }
}
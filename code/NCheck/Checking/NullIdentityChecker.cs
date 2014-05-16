namespace NCheck.Checking
{
    using System;

    /// <summary>
    /// Implements a default identity checker.
    /// </summary>
    public class NullIdentityChecker : IIdentityChecker
    {
        /// <copydocfrom cref="IIdentityChecker.SupportsId" />
        public bool SupportsId(Type type)
        {
            return false;
        }

        /// <copydocfrom cref="IIdentityChecker.ExtractId" />
        public object ExtractId(object value)
        {
            return null;
        }
    }
}
using NCheck.Checking;
using NCheck.Test.Checking;

namespace NCheck.Test
{
    public class CheckerFactory : NCheck.CheckerFactory
    {
        public CheckerFactory()
        {
            // NB Deliberate virtual call so we invoke AssignConventions in the most derived CheckerFactory.
            AssignConventions();

            Register(typeof(CheckerFactory).Assembly);
            Register(typeof(NCheck.CheckerFactory).Assembly);
        }

        /// <summary>
        /// Can assigns the conventions for the instance or configure ConventionsFactory as needed.
        /// </summary>
        protected virtual void AssignConventions()
        {
            if (ConventionsFactory.FactoryType == null)
            {
                // Ok, first time through so set it up
                ConventionsFactory.IdentityCheckerFactory = () => new IdentifiableChecker();
                ConventionsFactory.TypeConventionsFactory = c => c.AssignTypeConventions();
                ConventionsFactory.PropertyConventionsFactory = c => c.AssignPropertyInfoConventions();
                ConventionsFactory.ComparerConventionsFactory = c => c.AssignComparerConventions();

                // Mark it as setup
                ConventionsFactory.FactoryType = GetType();
            }

            // Sanity check - only needed if you are changing conventions on a per-test basis
            if (Conventions.IdentityChecker is IdentifiableChecker)
            {
                // Assume it's ok
                return;
            }

            Conventions.IdentityChecker = new IdentifiableChecker();
        }
    }
}
namespace NCheck.Test
{
    using System.Reflection;

    using NCheck.Checking;

    public class CheckerFactory : NCheck.CheckerFactory
    {
        public CheckerFactory()
        {
            PropertyCheck.IdentityChecker = new IdentifiableChecker();

            Initialize();
        }

        private void Initialize()
        {
            // NB Conventions must be before type registrations if they are to apply.
            Convention(x => typeof(IIdentifiable).IsAssignableFrom(x) ? CompareTarget.Id : CompareTarget.Unknown);
            Convention((PropertyInfo x) => x.Name == "Ignore" ? CompareTarget.Ignore : CompareTarget.Unknown);

            Register(typeof(CheckerFactory).Assembly);
            Register(typeof(NCheck.CheckerFactory).Assembly);
        }
    }
}
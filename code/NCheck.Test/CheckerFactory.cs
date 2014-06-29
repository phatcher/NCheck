namespace NCheck.Test
{
    using System;
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
            Convention(x => typeof(IIdentifiable).IsAssignableFrom(x), CompareTarget.Id);
            Convention((PropertyInfo x) => x.Name == "Ignore", CompareTarget.Ignore);

            // NB We have an extension to use a function for a type or we can do it explicitly if we want more context
            ComparerConvention<double>(AbsDouble);
            ComparerConvention<double>(x => (x == typeof(double)), AbsDouble);

            Register(typeof(CheckerFactory).Assembly);
            Register(typeof(NCheck.CheckerFactory).Assembly);
        }

        public bool AbsDouble(double x, double y)
        {
            return Math.Abs(x - y) < 0.001;
        }

        public bool AbsFloat(float x, float y)
        {
            return Math.Abs(x - y) < 0.001;
        }
    }
}
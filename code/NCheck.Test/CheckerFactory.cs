using System;
using System.Reflection;

using NCheck.Checking;

namespace NCheck.Test
{
    public class CheckerFactory : NCheck.CheckerFactory
    {
        public CheckerFactory(bool clear = false) : base()
        {
            // Only continue if we've been asked to clear or we are not initialized yet.
            if (clear || !(PropertyCheck.IdentityChecker is IdentifiableChecker))
            {
                ConfigureConventions();
            }

            Initialize(clear);
        }

        private void Initialize(bool clear)
        {
            Register(typeof(CheckerFactory).Assembly);
            Register(typeof(NCheck.CheckerFactory).Assembly);
        }

        public static void ConfigureConventions()
        {
            PropertyCheck.IdentityChecker = new IdentifiableChecker();

            // NB Conventions must be before type registrations if they are to apply.
            PropertyCheck.Convention(x => typeof(IIdentifiable).IsAssignableFrom(x), CompareTarget.Id);
            PropertyCheck.Convention((PropertyInfo x) => x.Name == "Ignore", CompareTarget.Ignore);

            // NB We have an extension to use a comparision function for a type or property or we can do it explicitly if we want more context
            PropertyCheck.ComparerConvention<double>(AbsDouble);
            //PropertyCheck.ComparerConvention<double>(x => x == typeof(double), AbsDouble);
            PropertyCheck.ComparerConvention<float>(AbsFloat);
            //PropertyCheck.ComparerConvention<float>(x => x == typeof(double), AbsFloat);
        }

        public static bool AbsDouble(double x, double y)
        {
            return Math.Abs(x - y) < 0.001;
        }

        public static bool AbsFloat(float x, float y)
        {
            return Math.Abs(x - y) < 0.001;
        }
    }
}
using System;
using System.Reflection;
using NCheck.Checking;

namespace NCheck.Test.Checking
{
    public static class ConventionExtensions
    {
        public static void AssignPropertyInfoConventions(this CheckerConventions conventions)
        {
            //conventions.Convention(x => typeof(IIdentifiable).IsAssignableFrom(x), CompareTarget.Id);
            conventions.Convention((PropertyInfo x) => x.Name == "Ignore", CompareTarget.Ignore);
        }

        public static void AssignTypeConventions(this CheckerConventions conventions)
        {
            // NB Must have this one to put base behaviour in suchs as Guid
            conventions.TypeConventions.InitializeTypeConventions();

            // NB Conventions must be after general type registrations if they are to apply.
            conventions.Convention(x => typeof(IIdentifiable).IsAssignableFrom(x), CompareTarget.Id);
        }

        public static void AssignComparerConventions(this CheckerConventions conventions)
        {
            // NB We have an extension to use a function for a type or we can do it explicitly if we want more context
            conventions.ComparerConvention<double>(AbsDouble);
            conventions.ComparerConvention<double?>(AbsDouble);

            conventions.ComparerConvention<float>(AbsFloat);
            conventions.ComparerConvention<float?>(AbsFloat);
            conventions.ComparerConvention<float>(x => (x == typeof(float)), AbsFloat);
        }

        public static bool AbsDouble(double? x, double? y)
        {
            return x.HasValue && y.HasValue && AbsDouble(x.Value, y.Value);
        }

        public static bool AbsDouble(double x, double y)
        {
            return NearlyEqual(x, y, 0.001);
        }

        public static bool AbsFloat(float? x, float? y)
        {
            return x.HasValue && y.HasValue && AbsFloat(x.Value, y.Value);
        }

        public static bool AbsFloat(float x, float y)
        {
            return NearlyEqual(x, y, 0.00001);
        }

        /// <summary>
        /// Compare two floats and check if they are approximately equal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        /// <remarks>http://stackoverflow.com/questions/3874627/floating-point-comparison-functions-for-c-sharp</remarks>
        public static bool NearlyEqual(float a, float b, float epsilon)
        {
            var absA = Math.Abs(a);
            var absB = Math.Abs(b);
            var diff = Math.Abs(a - b);

            if (a == b)
            {
                // shortcut, handles infinities
                return true;
            }

            if (a == 0 || b == 0 || diff < float.Epsilon)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < epsilon;
            }

            // use relative error
            return diff / (absA + absB) < epsilon;
        }

        public static bool NearlyEqual(double a, double b, double epsilon)
        {
            var absA = Math.Abs(a);
            var absB = Math.Abs(b);
            var diff = Math.Abs(a - b);

            if (a == b)
            {
                // shortcut, handles infinities
                return true;
            }

            if (a == 0 || b == 0 || diff < double.Epsilon)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < epsilon;
            }

            // use relative error
            return diff / (absA + absB) < epsilon;
        }
    }
}

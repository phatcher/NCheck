namespace NCheck.Checking
{
    using System;

    /// <copydocfrom cref="IConventions{T}" />
    public class Conventions<TSource> : IConventions<TSource>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Conventions{T}" /> class.
        /// </summary>
        public Conventions(CompareTarget defaultCompareTarget = Checking.CompareTarget.Unknown)
        {
            Comparer = new Conventions<TSource, Func<object, object, bool>>();
            CompareTarget = new Conventions<TSource, CompareTarget> { DefaultConvention = defaultCompareTarget };
        }

        /// <copydocfrom cref="IConventions{T}.Clear" />
        public void Clear()
        {
            Comparer.Clear();
            CompareTarget.Clear();
        }

        /// <copydocfrom cref="IConventions{T}.Comparer" />
        public IConventions<TSource, Func<object, object, bool>> Comparer { get; private set; }

        /// <copydocfrom cref="IConventions{T}.CompareTarget" />
        public IConventions<TSource, CompareTarget> CompareTarget { get; private set; }
    }
}
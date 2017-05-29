using System;
using System.Collections.Generic;

namespace NCheck.Checking
{
    /// <copydocfrom cref="IConventions{TSource, TTarget}" />
    public class Conventions<TSource, TTarget> : IConventions<TSource, TTarget>
    {
        private readonly List<KeyValuePair<Func<TSource, bool>, TTarget>> conventions;

        /// <summary>
        /// Creates a new instance of the <see cref="Conventions{T, U}" /> class.
        /// </summary>
        public Conventions()
        {
            conventions = new List<KeyValuePair<Func<TSource, bool>, TTarget>>();
            DefaultConvention = default(TTarget);
        }

        /// <summary>
        /// Gets the target value if no convention holds
        /// </summary>
        public TTarget DefaultConvention { get; set; }

        /// <copydocfrom cref="IConventions{T}" />
        public void Clear()
        {
            conventions.Clear();
        }

        /// <copydocfrom cref="IConventions{T}" />
        public TTarget Convention(TSource source)
        {
            foreach (var kvp in conventions)
            {
                if (kvp.Key(source))
                {
                    return kvp.Value;
                }
            }

            return DefaultConvention;
        }

        /// <copydocfrom cref="IConventions{T}" />
        public void Register(Func<TSource, bool> func, TTarget value)
        {
            conventions.Insert(0, new KeyValuePair<Func<TSource, bool>, TTarget>(func, value));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace NCheck.Checking
{
    /// <copydocfrom cref="IConventions{TSource, TTarget}" />
    public class Conventions<TSource, TTarget> : IConventions<TSource, TTarget>
    {
        private readonly List<KeyValuePair<Func<TSource, bool>, TTarget>> conventions;
        private readonly object syncLock;

        /// <summary>
        /// Creates a new instance of the <see cref="Conventions{T, U}" /> class.
        /// </summary>
        public Conventions()
        {
            conventions = new List<KeyValuePair<Func<TSource, bool>, TTarget>>();
            syncLock = new object();
            DefaultConvention = default(TTarget);
        }

        /// <summary>
        /// Gets the target value if no convention holds
        /// </summary>
        public TTarget DefaultConvention { get; set; }

        /// <copydocfrom cref="IConventions{T}" />
        public void Clear()
        {
            lock (syncLock)
            {
                conventions.Clear();
            }
        }

        /// <copydocfrom cref="IConventions{T}" />
        public TTarget Convention(TSource source)
        {
            // Snapshot the list so we can handle if another thread changes it
            foreach (var kvp in conventions.ToList())
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
            lock (syncLock)
            {
                conventions.Insert(0, new KeyValuePair<Func<TSource, bool>, TTarget>(func, value));
            }
        }
    }
}
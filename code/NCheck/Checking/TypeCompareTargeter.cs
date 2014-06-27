namespace NCheck.Checking
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Determines a compare target to use for a type.
    /// </summary>
    public class TypeCompareTargeter : ITypeCompareTargeter
    {
        private readonly List<Func<Type, CompareTarget>> targeters;
        
        /// <summary>
        /// Creates a new instance of the <see cref="TypeCompareTargeter" />
        /// </summary>
        public TypeCompareTargeter()
        {
            targeters = new List<Func<Type, CompareTarget>>();
        }

        /// <copydocfrom cref="ITypeCompareTargeter.Clear" />
        public void Clear()
        {
            targeters.Clear();
        }

        /// <summary>
        /// Determine which <see cref="CompareTarget"/> to use for a type.
        /// <para>
        /// Mostly obvious i.e. reference types use <see cref="CompareTarget.Entity"/> and primitive values types <see cref="CompareTarget.Value"/>, but
        /// we default structs to Entity so that we can inspect the individual members, see <see cref="CheckerExtensions.InitializeTypeCompareTargeter" />
        /// </para>
        /// </summary>
        /// <param name="type">Type to use</param>
        /// <returns><see cref="CompareTarget"/> to use</returns>
        public CompareTarget DetermineCompareTarget(Type type)
        {
            foreach (var func in targeters)
            {
                var result = func(type);
                if (result != CompareTarget.Unknown)
                {
                    return result;
                }
            }

            return CompareTarget.Value;
        }

        /// <copydocfrom cref="ITypeCompareTargeter.Register(Type, CompareTarget)" />
        public void Register(Type type, CompareTarget target)
        {
            Register(x => x.FullName == type.FullName ? target : CompareTarget.Unknown);
        }

        /// <copydocfrom cref="ITypeCompareTargeter.Register(Func{Type, CompareTarget})" />
        /// <remarks>More recent functions take precedence when determining the compare target</remarks>
        public void Register(Func<Type, CompareTarget> func)
        {
            // NB Always insert at the top so most recent registration wins.
            targeters.Insert(0, func);
        }
    }
}
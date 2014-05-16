namespace NCheck.Checking
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Determines a compare target to use for a type.
    /// </summary>
    public class TypeCompareTargeter : ITypeCompareTargeter
    {
        private readonly Dictionary<Type, CompareTarget> types;

        /// <summary>
        /// Creates a new instance of the <see cref="TypeCompareTargeter" />
        /// </summary>
        public TypeCompareTargeter()
        {
            types = new Dictionary<Type, CompareTarget>();

            // Put some basic ones in there
            Register(typeof(Guid), CompareTarget.Value);
            Register(typeof(String), CompareTarget.Value);
            Register(typeof(Decimal), CompareTarget.Value);
            Register(typeof(Single), CompareTarget.Value);
            Register(typeof(Double), CompareTarget.Value);
            Register(typeof(DateTime), CompareTarget.Value);
            Register(typeof(DateTimeOffset), CompareTarget.Value);
            Register(typeof(TimeSpan), CompareTarget.Value);
            Register(typeof(TimeZone), CompareTarget.Value);
            Register(typeof(TimeZoneInfo), CompareTarget.Value);

            // Type is abstract, get RuntimeType at r/t which is internal so can't handle easily
            Register(typeof(Type), CompareTarget.Value);
        }

        /// <summary>
        /// Determine which <see cref="CompareTarget"/> to use for a type.
        /// <para>
        /// Mostly obvious i.e. reference types use <see cref="CompareTarget.Entity"/> and primitive values types <see cref="CompareTarget.Value"/>, but
        /// we default structs to Entity so that we can inspect the individual members.
        /// </para>
        /// </summary>
        /// <param name="type">Type to use</param>
        /// <returns><see cref="CompareTarget"/> to use</returns>
        public CompareTarget DetermineCompareTarget(Type type)
        {
            CompareTarget compareTarget;

            if (types.TryGetValue(type, out compareTarget))
            {
                return compareTarget;
            }

            if (type.IsEnum)
            {
                compareTarget = CompareTarget.Value;
            }
            else if (type.IsValueType)
            {
                // Trying to make sure we process structs as entities, but exclude nullables
                if (!type.IsPrimitive && !type.FullName.StartsWith("System.Nullable"))
                {
                    compareTarget = CompareTarget.Entity;
                }
            }
            else
            {
                // Unless it's enumerable we will assume its an entity since all the primitive types
                // and exceptions have been dealt with in the constructor.
                compareTarget = typeof(IEnumerable).IsAssignableFrom(type) ? CompareTarget.Collection : CompareTarget.Entity;
            }

            return compareTarget;
        }

        /// <summary>
        /// Register an explicit <see cref="CompareTarget"/> to use for a type.
        /// </summary>
        /// <param name="type">Type to use</param>
        /// <param name="target">CompareTarget to use</param>
        public void Register(Type type, CompareTarget target)
        {
            types.Add(type, target);
        }
    }
}
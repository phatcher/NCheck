namespace NCheck.Checking
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Checks a single property on a class.
    /// </summary>
    public class PropertyCheck
    {
        private static Action<IConventions<Type>> initializer;
        private static IConventions<Type> typeConventions;
        private CompareTarget compareTarget;
        private Func<object, object, bool> comparer;

        static PropertyCheck()
        {
            SetTypeConventionsInitializer(CheckerExtensions.InitializeTypeConventions);
        }

        /// <summary>
        /// Create a new instance of the <see cref="PropertyCheck" /> class
        /// </summary>
        /// <param name="info">PropertyInfo to use</param>
        /// <param name="compareTarget">CompareTarget to use</param>
        public PropertyCheck(PropertyInfo info, CompareTarget compareTarget)
        {
            Info = info;
            CompareTarget = compareTarget;
        }

        /// <summary>
        /// Gets or sets the class which knows how to extract an Id from an object
        /// </summary>
        public static IIdentityChecker IdentityChecker { get; set; }

        /// <summary>
        /// Gets or sets the class which knows the default <see cref="CompareTarget"/> for a property.
        /// <para>
        /// This allows the introduction of conventions based on property names.
        /// </para>
        /// </summary>
        public static IConventions<PropertyInfo> PropertyConventions { get; set; }

        /// <summary>
        /// Gets or sets the class which knows the conventions for a type.
        /// </summary>
        public static IConventions<Type> TypeConventions 
        {
            get { return typeConventions; }
            set
            {
                typeConventions = value;
                initializer(typeConventions);
            }
        }

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> used to access values on the object.
        /// </summary>
        public PropertyInfo Info { get; private set; }

        /// <summary>
        /// Gets or sets the comparer use to determine equality for <see cref="NCheck.Checking.CompareTarget.Id"/> and <see cref="NCheck.Checking.CompareTarget.Value"/>
        /// </summary>
        /// <remarks>Default method is object.Equals</remarks>
        public Func<object, object, bool> Comparer 
        {
            get { return comparer ?? (comparer = object.Equals); }
            set { comparer = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="CompareTarget" /> used to determine what type of comparison
        /// to perform on the <see cref="PropertyInfo" /> derived values.
        /// </summary>
        public CompareTarget CompareTarget
        {
            get { return compareTarget; }
            set
            {
                compareTarget = value;
                OnCompareTargetChanged();
            }
        }
        /// <summary>
        /// Gets or sets the length property, used to limit string comparisons.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Sets the function that initializes the <see cref="TypeConventions"/>
        /// </summary>
        /// <param name="func"></param>
        public static void SetTypeConventionsInitializer(Action<IConventions<Type>> func)
        {
            initializer = func;
        }

        /// <summary>
        /// Determine the comparison target for a property.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static CompareTarget DetermineCompareTarget(PropertyInfo propertyInfo)
        {
            CompareTarget target;

            if (PropertyConventions != null)
            {
                target = PropertyConventions.CompareTarget.Convention(propertyInfo);
                if (target != CompareTarget.Unknown)
                {
                    return target;
                }
            }

            if (TypeConventions == null)
            {
                throw new NotSupportedException("No type conventions assigned to PropertyCheck");
            }

            target = TypeConventions.CompareTarget.Convention(propertyInfo.PropertyType);

            return target != CompareTarget.Unknown ? target : CompareTarget.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checker"></param>
        /// <param name="expectedEntity"></param>
        /// <param name="candidateEntity"></param>
        /// <param name="objectName"></param>
        public void Check(IChecker checker, object expectedEntity, object candidateEntity, string objectName)
        {
            // Early quit if told to ignore.
            if (CompareTarget == CompareTarget.Ignore)
            {
                return;
            }

            // Have to do the null check here, else "System.Reflection.TargetException: Non-static method requires a target."
            // could be thrown by RuntimePropertyInfo.GetValue().
            if (CheckNullNotNull(expectedEntity, candidateEntity, objectName))
            {
                return;
            }

            var expectedValue = ExtractValue(expectedEntity, objectName + ".Expected");
            var candidateValue = ExtractValue(candidateEntity, objectName + ".Expected");

            Check(CompareTarget, checker, expectedValue, candidateValue, objectName + "." + Info.Name);
        }

        /// <summary>
        /// Check the Id property of the entities.
        /// </summary>
        /// <param name="expected">Expected value</param>
        /// <param name="candidate">Candidate value</param>
        /// <param name="objectName">Name of the object we are checking</param>
        protected void CheckId(object expected, object candidate, string objectName)
        {
            // If both null we are ok, and can't check the Id property so quit now.
            if (CheckNullNotNull(expected, candidate, objectName))
            {
                return;
            }

            if (IdentityChecker == null)
            {
                throw new NotSupportedException("No IdentityChecker assigned, cannot perform Id check");
            }

            var expectedId = IdentityChecker.ExtractId(expected);
            var candidateId = IdentityChecker.ExtractId(candidate);
            Check(expectedId, candidateId, objectName);
        }

        /// <summary>
        /// Extracts the value from an entity.
        /// </summary>
        /// <param name="entity">Entity to use</param>
        /// <param name="objectName">Name of the object we are checking</param>
        /// <returns>Property value of the entity.</returns>
        protected object ExtractValue(object entity, string objectName)
        {
            try
            {
                return Info.GetValue(entity, null);
            }
            catch (Exception ex)
            {
                throw new PropertyCheckException(objectName + "." + Info.Name, string.Empty, ex.Message);
            }
        }

        /// <summary>
        /// Verify that the compare target is valid. 
        /// </summary>
        protected void OnCompareTargetChanged()
        {
            if (CompareTarget != CompareTarget.Id)
            {
                return;
            }

            if (IdentityChecker == null)
            {
                throw new NotSupportedException("No IdentityChecker assigned, cannot perform Id check");
            }

            if (!IdentityChecker.SupportsId(Info.PropertyType))
            {
                throw new NotSupportedException(string.Format("Property {0}: type ({1}) must support Id check", Info.Name, Info.PropertyType));
            }
        }

        /// <summary>
        /// Apply a <see cref="CompareTarget"/> to a checker for some entities.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="checker"></param>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="objectName"></param>
        protected void Check(CompareTarget target, IChecker checker, object expected, object candidate, string objectName)
        {
            switch (target)
            {
                case CompareTarget.Ignore:
                    break;

                case CompareTarget.Id:
                    CheckId(expected, candidate, objectName + ".Id");
                    break;

                case CompareTarget.Entity:
                    checker.Check(expected, candidate, objectName);
                    break;

                case CompareTarget.Count:
                case CompareTarget.Collection:
                    Check(checker, expected as IEnumerable, candidate as IEnumerable, objectName);
                    break;

                case CompareTarget.Value:
                    Check(expected, candidate, objectName);
                    break;

                default:
                    throw new NotSupportedException("Cannot perform comparison: " + target);
            }
        }

        /// <summary>
        /// Check if two collections of <see typeparamref="T" /> are equal.
        /// </summary>
        /// <remarks>
        /// The parameters are first checked for null, an exception is thrown if only one is null.
        /// Second, the cardinalities of the collections are checked if the <see cref="IEnumerable{T}" /> is
        /// also <see cref="ICollection{T}" /> which means that it supports <see cref="ICollection{T}.Count" />
        /// If these checks are passed, each item is compared in turn.
        /// </remarks>
        /// <param name="checker"></param>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="objectName"></param>
        protected void Check(IChecker checker, IEnumerable expected, IEnumerable candidate, string objectName)
        {
            // Do we have two actual lists
            if (CheckNullNotNull(expected, candidate, objectName))
            {
                return;
            }

            CheckCardinality(expected, candidate, objectName);
            if (CompareTarget == CompareTarget.Count)
            {
                // We're done
                return;
            }

            // Ok, step both iterator togeter, will work as these are now confirmed to have the same cardinality
            var i = 0;
            var enumExpected = expected.GetEnumerator();
            var enumCandidate = candidate.GetEnumerator();
            var target = CompareTarget.Entity;
            Type type = null;

            enumExpected.Reset();
            enumCandidate.Reset();
            while (enumExpected.MoveNext())
            {
                if (type == null)
                {
                    type = enumExpected.Current.GetType();
                    if (TypeConventions == null)
                    {
                        throw new NotSupportedException("No type conventions assigned to PropertyCheck");
                    }
                    target = TypeConventions.CompareTarget.Convention(type);
                }
                enumCandidate.MoveNext();
                Check(target, checker, enumExpected.Current, enumCandidate.Current, objectName + "[" + i++ + "]");
            }
        }
        
        /// <summary>
        /// Check if both expected and candidate are null or not null
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private static bool CheckNullNotNull(object expected, object candidate, string objectName)
        {
            if (expected == null && candidate == null)
            {
                return true;
            }

            if (expected == null)
            {
                throw new PropertyCheckException(objectName, "null", "not null");
            }

            if (candidate == null)
            {
                throw new PropertyCheckException(objectName, "not null", "null");
            }

            return false;
        }

        private static void CheckCardinality(IEnumerable expected, IEnumerable candidate, string objectName)
        {
            var expectedList = expected as ICollection;
            var candidateList = candidate as ICollection;

            // Sanity check to see if they are collections (could be just IEnumerable)
            if (expectedList == null || candidateList == null)
            {
                return;
            }

            if (expectedList.Count != candidateList.Count)
            {
                throw new PropertyCheckException(objectName + ".Count", expectedList.Count, candidateList.Count);
            }
        }

        private void Check(object expected, object candidate, string objectName)
        {
            if (CheckNullNotNull(expected, candidate, objectName))
            {
                return;
            }

            try
            {
                if (!Comparer(expected, candidate))
                {
                    throw new PropertyCheckException(objectName, expected, candidate);
                }
            }
            catch (PropertyCheckException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new PropertyCheckException(objectName, string.Empty, ex.Message);
            }
        }
    }
}
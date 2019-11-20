using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace NCheck.Checking
{
    /// <summary>
    /// Checks a single property on a class.
    /// </summary>
    public class PropertyCheck
    {
        private CheckerConventions conventions;
        private CompareTarget compareTarget;
        private Func<object, object, bool> comparer;

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
        /// Gets or sets the class which knows the conventions.
        /// </summary>
        public CheckerConventions Conventions
        {
            get => conventions ?? (conventions = ConventionsFactory.Conventions);
            set => conventions = value;
        }

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> used to access values on the object.
        /// </summary>
        public PropertyInfo Info { get; }

        /// <summary>
        /// Gets or sets the comparer use to determine equality for <see cref="Checking.CompareTarget.Id"/> and <see cref="NCheck.Checking.CompareTarget.Value"/>
        /// </summary>
        /// <remarks>Default method is object.Equals</remarks>
        public Func<object, object, bool> Comparer
        {
            get => comparer ?? (comparer = Equals);
            set => comparer = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="CompareTarget" /> used to determine what type of comparison
        /// to perform on the <see cref="PropertyInfo" /> derived values.
        /// </summary>
        public CompareTarget CompareTarget
        {
            get => compareTarget;
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
            var candidateValue = ExtractValue(candidateEntity, objectName + ".Candidate");

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

            if (Conventions.IdentityChecker == null)
            {
                throw new NotSupportedException("No IdentityChecker assigned, cannot perform Id check");
            }

            var expectedId = Conventions.IdentityChecker.ExtractId(expected);
            var candidateId = Conventions.IdentityChecker.ExtractId(candidate);
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
            if (CompareTarget == CompareTarget.Value)
            {
                // Use property comparer in preference to type comparer
                Comparer = Conventions.PropertyConventions.Comparer.Convention(Info) ??
                           Conventions.TypeConventions.Comparer.Convention(Info.PropertyType);
                return;
            }

            if (CompareTarget != CompareTarget.Id)
            {
                return;
            }

            if (Conventions.IdentityChecker == null)
            {
                throw new NotSupportedException("No IdentityChecker assigned, cannot perform Id check");
            }

            if (!Conventions.IdentityChecker.SupportsId(Info.PropertyType))
            {
                throw new NotSupportedException($"Property {Info.Name}: type ({Info.PropertyType}) must support Id check");
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
        protected void Check(CompareTarget target, IChecker checker, object expected, object candidate,
            string objectName)
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

                case CompareTarget.Dictionary:
                    Check(checker, expected as IDictionary, candidate as IDictionary, objectName);
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
        /// Check if two collections are equal.
        /// </summary>
        /// <remarks>
        /// The parameters are first checked for null, an exception is thrown if only one is null.
        /// Second, the cardinalities of the collections are checked if the <see cref="IEnumerable" /> is
        /// also <see cref="ICollection" /> which means that it supports <see cref="ICollection.Count" />
        /// If these checks are passed, each item is compared in turn.
        /// </remarks>
        /// <param name="checker"></param>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="objectName"></param>
        protected void Check(IChecker checker, IEnumerable expected, IEnumerable candidate, string objectName)
        {
            // Do we have two lists
            if (CheckNullNotNull(expected, candidate, objectName))
            {
                return;
            }

            var tracker = new ExceptionTracker(objectName);
            var canIterate = CheckCardinality(tracker, expected, candidate, objectName);
            if (CompareTarget != CompareTarget.Count && canIterate)
            {
                // Ok, step both iterators together, might not work as they could have a different cardinality
                var i = 0;
                var enumExpected = expected.GetEnumerator();
                var enumCandidate = candidate.GetEnumerator();

                enumExpected.Reset();
                enumCandidate.Reset();
                try
                {
                    while (enumExpected.MoveNext())
                    {
                        enumCandidate.MoveNext();

                        CheckElement(tracker, checker, enumExpected.Current, enumCandidate.Current, $"[{i++}]");
                    }
                }
                catch (InvalidOperationException ex)
                {
                    if (ex.Message != "Enumeration has either not started or has already finished.")
                    {
                        throw;
                    }
                }
            }

            var p = tracker.Report(objectName);
            if (p != null)
            {
                throw p;
            }
        }

        /// <summary>
        /// Check if two <see cref="IDictionary"/> are equal.
        /// </summary>
        /// <param name="checker"></param>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="objectName"></param>
        protected void Check(IChecker checker, IDictionary expected, IDictionary candidate, string objectName)
        {
            // Do we have two dictionaries
            if (CheckNullNotNull(expected, candidate, objectName))
            {
                return;
            }

            var tracker = new ExceptionTracker(objectName);

            var keys = new List<object>();
            foreach (var key in expected.Keys)
            {
                // Track the processed keys
                keys.Add(key);

                // Does it exist
                if (!candidate.Contains(key))
                {
                    tracker.Add($"[{key}]", expected[key], "null");
                }
                else
                {
                    CheckElement(tracker, checker, expected[key], candidate[key], $"[{key}]");
                }
            }

            // Now do the reverse, excluding keys we've already processed            
            foreach (var key in candidate.Keys)
            {
                if (keys.Contains(key))
                {
                    // Tracked it the first time around
                    continue;
                }

                tracker.Add($"[{key}]", "null", candidate[key]);
            }

            var p = tracker.Report(objectName);
            if (p != null)
            {
                throw p;
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

        private static bool CheckCardinality(ExceptionTracker tracker, IEnumerable expected, IEnumerable candidate, string objectName)
        {
            // Sanity check to see if they are collections (could be just IEnumerable)
            if (!(expected is ICollection expectedList) || !(candidate is ICollection candidateList))
            {
                // Can't iterate 
                return false;
            }

            if (expectedList.Count != candidateList.Count)
            {
                tracker.Add("Count", expectedList.Count, candidateList.Count);
            }
            var canIterate = expectedList.Count != 0 && candidateList.Count != 0;

            return canIterate;
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

        private void CheckElement(ExceptionTracker tracker, IChecker checker, object expected, object candidate, string objectName)
        {
            try
            {
                var target = ElementCompareType(expected, candidate, objectName);

                Check(target, checker, expected, candidate, objectName);
            }
            catch (PropertyCheckException pex)
            {
                tracker.Add(pex);
            }
        }

        private CompareTarget ElementCompareType(object expected, object candidate, string objectName)
        {
            if (CheckNullNotNull(expected, candidate, objectName))
            {
                return CompareTarget.Value;
            }

            if (Conventions.TypeConventions == null)
            {
                throw new NotSupportedException("No type conventions assigned to PropertyCheck");
            }

            var value = expected ?? candidate;
            var type = value?.GetType();
            var target = Conventions.TypeConventions.CompareTarget.Convention(type);

            return target;
        }
    }
}
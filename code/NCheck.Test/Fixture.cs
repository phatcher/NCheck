﻿namespace NCheck.Test
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    /// <summary>
    /// Base test fixture that sets up a checker factory
    /// </summary>
    public class Fixture
    {
        private ICheckerFactory checkerFactory;

        /// <summary>
        /// Gets the checker factory.
        /// </summary>
        /// <remarks>Creates it on first use and also assigns ServiceLocator.Current to our ServiceLocator.</remarks>
        public ICheckerFactory CheckerFactory
        {
            get
            {
                if (checkerFactory == null)
                {
                    checkerFactory = CreateCheckerFactory();
                    if (checkerFactory == null)
                    {
                        throw new NotSupportedException("No CheckerFactory assigned to fixture");
                    }
                }

                return checkerFactory;
            }
        }

        /// <summary>
        /// Pre-test set up.
        /// </summary>
        [SetUp]
        public virtual void Setup()
        {
            TidyUp();
            OnSetup();
        }

        /// <summary>
        /// Post-test tidy up
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
            OnTearDown();
            TidyUp();
        }

        /// <summary>
        /// Verify that the state of a couple of objects is the same
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity we want to check</typeparam>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        protected void Check<TEntity>(TEntity expected, TEntity candidate)
        {
            Check(expected, candidate, typeof(TEntity).Name);
        }

        /// <summary>
        /// Verify that the state of a couple of objects is the same
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="objectName"></param>
        protected void Check<TEntity>(TEntity expected, TEntity candidate, string objectName)
        {
            CheckerFactory.Check(expected, candidate, objectName);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        protected void Check<T>(IList<T> expectedList, IList<T> candidateList)
        {
            Check(expectedList, candidateList, string.Empty);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        /// <param name="objectName"></param>
        protected void Check<T>(IList<T> expectedList, IList<T> candidateList, string objectName)
        {
            CheckerFactory.Check(expectedList as IEnumerable<T>, candidateList, objectName);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        protected void Check<T>(ICollection<T> expectedList, ICollection<T> candidateList)
        {
            Check(expectedList, candidateList, string.Empty);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        /// <param name="objectName"></param>
        protected void Check<T>(ICollection<T> expectedList, ICollection<T> candidateList, string objectName)
        {
            CheckerFactory.Check(expectedList as IEnumerable<T>, candidateList, objectName);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        protected void Check<T>(IEnumerable<T> expectedList, IEnumerable<T> candidateList)
        {
            Check(expectedList, candidateList, string.Empty);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        /// <param name="objectName"></param>
        protected void Check<T>(IEnumerable<T> expectedList, IEnumerable<T> candidateList, string objectName)
        {
            CheckerFactory.Check(expectedList, candidateList, objectName);
        }

        protected virtual ICheckerFactory CreateCheckerFactory()
        {
            return new CheckerFactory();
        }

        /// <summary>
        /// Test specific setup logic, should call base.OnSetup when used
        /// </summary>
        protected virtual void OnSetup()
        {
        }

        /// <summary>
        /// Test specific tear down logic.
        /// </summary>
        protected virtual void OnTearDown()
        {
        }

        /// <summary>
        /// Clear down the test context.
        /// </summary>
        protected virtual void TidyUp()
        {
            // Ensure that we wipe down the core objects - NUnit re-uses the instance for all tests
            checkerFactory = null;
        }
    }
}
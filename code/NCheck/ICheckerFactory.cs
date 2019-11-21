﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using NCheck.Checking;

namespace NCheck
{
    /// <summary>
    /// Delivers <see cref="IChecker{T}" />s used to verify that two instances of <see typeparamref="T" /> are 
    /// the same on a per property basis.
    /// </summary>
    public interface ICheckerFactory : IChecker
    {
        /// <summary>
        /// Add comparison to an entity checker.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        PropertyCheckExpression Compare<T>(Expression<Func<T, object>> propertyExpression);

        /// <summary>
        /// Check that the properties of two instances of <see typeparamref="T" /> are equal
        /// </summary>
        /// <typeparam name="T">Type to check</typeparam>
        /// <param name="expected">Expected object to use</param>
        /// <param name="candidate">Candidate object to use</param>
        /// <param name="objectName">Name to use, displayed in error messages to disambiguate</param>
        void Check<T>(T expected, T candidate, string objectName = "");

        /// <summary>
        /// Check if two collections of <see typeparamref="T" /> are equal.
        /// </summary>
        /// <remarks>
        /// The parameters are first checked for null, an exception is thrown if only one is null.
        /// Second, the cardinalities of the collections are checked if the <see cref="IEnumerable{T}" /> is
        /// also <see cref="ICollection{T}" /> which means that it supports <see cref="ICollection{T}.Count" />
        /// If these checks are passed, each item is compared in turn.
        /// </remarks>
        /// <typeparam name="T">Type to check</typeparam>
        /// <param name="expectedList">Expected object to use</param>
        /// <param name="candidateList">Candidate object to use</param>
        /// <param name="objectName">Name to use, displayed in error messages to disambiguate</param>
        void Check<T>(IEnumerable<T> expectedList, IEnumerable<T> candidateList, string objectName = "");

        /// <summary>
        /// Check that the properties of the parent class of <see typeparamref="T" /> are equal.
        /// </summary>
        /// <remarks>
        /// Only used when testing a class which is part of a hierarchy
        /// </remarks>
        /// <typeparam name="T">Type to check</typeparam>
        /// <param name="expected">Expected object to use</param>
        /// <param name="candidate">Candidate object to use</param>
        /// <param name="objectName">Name to use, displayed in error messages to disambiguate</param>
        void CheckParent<T>(T expected, T candidate, string objectName = "");
    }
}
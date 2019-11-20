#### 4.0.0 (2019-11-20)
* Rework dictionary/list checking to get per-element reporting
* Support net472, net48 and netstandard2.0 
* SourceLink support
* Internal refactoring for clarity and removed obsolete code

#### 3.0.0 (2018-05-10)
* Support net35, net452, net462 and netstandard1.5 and above

#### 2.3.2 (2018-01-21)
* Update tooling to VS2017

#### 2.3.1 (2017-11-06)
* Locking for conventions and checker, trying to support NUnit Parallelizable

#### 2.3.0 (2017-05-29)
* Introduce new technique for wiring up conventions, CheckerConventions 
* Supports multi-threaded use e.g. test fixtures using NUnit Parallelizable

#### 2.2.3 (2017-04-18)
* Introduce Dictionary compare type to PropertyCheck with per-key comparision of IDictionary/IDictionary<K, V>

#### 2.2.2 (2017-02-16)
* Fix automatic assignment of custom comparers to PropertyCheck

#### 2.2.1 (2016-11-04)
* Fix Checker<T> bug when T is a generic interface and no parent type exists.

#### 2.2.0 (2015-05-28)
* Sign the assembly
* BUGFIX: Detailed reporting of InvalidCastException when comparing values rather than naked exception.
* Extend ICheckerFactory to support Compare<T>, this allows for overriding checkers on a per-test basis.

#### 2.1.0 (2014-07-28)
* Fixes #4 - Incorrect label candidate/expected 
* Set the static Checker.CheckerFactory in Fixture.CheckerFactory in case it's not set by CheckerBuilder 
* Improve Fixture.CheckFault overrides so we can detect specific exception types as well as messages 
* Additional unit tests

#### 2.0.0 (2014-06-29)
* Reworked the ITypeCompareTargeter/IPropertyCompareTargeter into more generic IConvention<TSource, TTarget>
* #2 - Introduced custom comparers and convention registration for them
* CompareTarget convention registration syntax simplified
* Retired Exclude use Compare(x => x.PropertyName).Ignore instead.

#### 1.2.0 (2014-06-28)
* Add .Ignore onto PropertyCheckExpression, so we can do Compare(x => x.Property).Ignore as well as Exclude(x => x.Property)
* Added unit tests for PropertyCheckExpression
* Fixed nuspec so we don't include the test dll

#### 1.1.0 (2014-06-15)
* Change default builder to CheckerBuilder from NullCheckerBuilder
* Refactor TypeCompareTargeter to be more modular and allow registration of custom functions.
* Introduced IPropertyCompareTargeter/PropertyCompareTargeter to allow for property-based overrides
* Introduced CheckerFactory.Convention methods as helpers for registering both type and property based overrides

#### 1.0.0 (2014-04-16)
* First release.
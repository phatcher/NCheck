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
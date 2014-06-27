#### 1.2 (2014-06-27)
* 1.2: Add .Ignore onto PropertyCheckExpression, so we can do Compare(x => x.Property).Ignore as well as Exclude(x => x.Property)
Added unit tests for PropertyCheckExpression

#### 1.1 (2014-06-15)
* 1.1: Change default builder to CheckerBuilder from NullCheckerBuilder
Refactor TypeCompareTargeter to be more modular and allow registration of custom functions.
Introduced IPropertyCompareTargeter/PropertyCompareTargeter to allow for property-based overrides
Introduced CheckerFactory.Convention methods as helpers for registering both type and property based overrides

#### 1.0 (2014-04-16)
* 1.0: First release.
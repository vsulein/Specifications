# Specifications.Exprementation 

[![Build Status](https://travis-ci.org/vsulein/Specifications.Exprementation.svg?branch=master)](https://travis-ci.org/vsulein/Specifications.Exprementation)

Installation
-------------

Lib is available as a NuGet package. You can install it using the NuGet Package Console window:

```
PM> Install-Package Hangfire
```
Or with dotnet cli

```
dotnet add package specifications.exprementation
```


Usage
-------------

After installation, create your own class inherited by SpecificationBase

Example:
```csharp
public class ByName: SpecificationBase<Entity>
{
    public ByName(string name) : base(t => t.Name = name) { }
}
```

And try it

```csharp
var entity = new Entity() { Name = "Jonh", Type = 1 }
var byName = new ByName("Jonh");
bool result = byName.IsSatisfiedBy(entity) // true
//work with IQueryable and implicitly convert to Expression or Delegate(Predicate)
using (var db = new AppContext) {
    var collectionWithEntity = db.Entities.Where(byName); //expression
    collectionWithEntity.AsEnumerable().Where(byName); //delegate
}

```

Specifications support logical operations: and, or, not:

And 

```csharp
var byName = new ByName("Jonh");
var byType = new ByType(1);

SpecificationBase<Entity> byNameAndType = byName & byType;
//or interface version
ISpecification<Entity> byNameAndTypeInterface = byName.And(byType);
```

Or
```csharp
var byName = new ByName("Jonh");
var byType = new ByType(1);
   
SpecificationBase<Entity> byNameAndType = byName | byType;
//or interface version
ISpecification<Entity> byNameAndTypeInterface = byName.Or(byType);

var entity = new Entity() { Name = "Jonh", Type = 1 }
var result = byNameAndType.IsSatisfiedBy(entity) //true

```
Not

```csharp
var byName = new ByName("Joseph");

SpecificationBase<Entity> notByNameJoseph = !byName;
//or interface version
ISpecification<Entity> byNameAndType = byName.Not();
var result = byNameAndType.IsSatisfiedBy(entity) //true
```

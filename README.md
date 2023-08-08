# QueryableSearchExtensions

QueryableSearchExtensions is a collection of C# extension methods that enables text-based searches in database queries.

## Usage

With this extension method, you can perform a text search on a `DbSet<T>` across multiple properties. Here's how to use it:

```csharp
using QueryableSearchExtensions;

// ...

var searchTerm = "text to search";
Expression<Func<YourClass, string>>[] propertySelectors = 
{
    x => x.FirstProperty,
    x => x.SecondProperty,
    // ...
};

var results = dbContext.YourEntities.GlobalSearchQuery(searchTerm, propertySelectors).ToList();

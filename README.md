# LINQ Query Syntax Buddy


## Problem

LINQ queries can be expressed using the _query syntax_ in C#:

```c#
var query =
    from x in Enumerable.Range(1, 10)
    where x % 2 == 0
    select x.ToString();
```

or by chaining extension methods:

```c#
var query =
    Enumerable.Range(1, 10)
              .Where(x => x % 2 == 0)
              .Select(x => x.ToString());
```

While the query syntax is purer and scales far better in terms of readability
(especially when queries get justifiably long), it requires restoring to a
mixed approach when parts of a query need to materialize some results. For
example, to put the results of the above query into an array, one can simply
chain a call to `ToArray()`. When chaining methods, the call fits in
naturally:

```c#
var array =
    Enumerable.Range(1, 10)
              .Where(x => x % 2 == 0)
              .Select(x => x.ToString())
              .ToArray();
```

When using the query syntax, one has resort to a mixed approach that doesn't
read as natural:

```c#
var array = (
        from x in Enumerable.Range(1, 10)
        where x % 2 == 0
        select x.ToString()
    ).ToArray();
```


## Solution

This library provides some helpers that can be used to align better with the
query syntax such that the example discussed in the problem section becomes
as simple as writing:

```c#
var array = Array(
    from x in Enumerable.Range(1, 10)
    where x % 2 == 0
    select x.ToString());
```

```c#
var dict = Dictionary(
    from x in Enumerable.Range(1, 10)
    where x % 2 == 0
    select KeyValuePair.Create(x, x.ToString()));
```


## Usage

Add the following _static import_:

```c#
using static LinqQuerySyntaxBuddy.Enumerable;
```

To get the results of a query in array, call the `Array` function with
query as its parameter:

```c#
var array = Array(
    from x in Enumerable.Range(1, 10)
    where x % 2 == 0
    select x.ToString());
```

Likewise, to build a dictionary from a query, make sure that the query
generates a [`KeyValuePair<,>`][kvp] sequence and then use that as the
argument of the `Dictionary` function:

```c#
var dict = Dictionary(
    from x in Enumerable.Range(1, 10)
    where x % 2 == 0
    select KeyValuePair.Create(x, x.ToString()));
```

To provide a key comparer, supply it as the first argument and the query as
the second:

```c#
var dict = Dictionary(StringComparer.Ordinal,
    from x in new[]
    {
        "foo", "bar", "baz", "qux", "quux", "quuz",
        "corge", "grault", "garply", "waldo", "fred",
        "plugh", "xyzzy", "thud",
    }
    select KeyValuePair.Create(x.ToString(), x));
```

Like `Array` and `Dictionary`, there is also `List` and `HashSet`.

See the source code for more.


  [kvp]: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.keyvaluepair-2
  [todict]: https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.todictionary?view=netcore-3.1#System_Linq_Enumerable_ToDictionary__2_System_Collections_Generic_IEnumerable___0__System_Func___0___1__System_Collections_Generic_IEqualityComparer___1__

# RangeMinimumQuery
Generic Range Minimum Query Implementation in .NET (using Segment Tree)

calculate each query in O(log N), 
preprocessing in O(N)

```csharp
var values = new int[] { 5, 3, 2, 1, 9 };
var rmq = new RMQ<int>(values);

Assert.Equal(1, rmq.Query(start: 0, end: 5));
Assert.Equal(2, rmq.Query(start: 0, end: 3));
```


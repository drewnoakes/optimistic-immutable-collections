# Optimistic Immutable Collections

Thread-safe collection types backed by `System.Collections.Immutable` that use lock-free operations to update their inner
state.

- `OptimisticList<T>`
- `OptimisticSet<T>`
- `OptimisticDictionary<TKey, TValue>`

These types implemente familiar collection interfaces. They hide the immutable collection API internally, other than when it provides useful capabilities or reduces allocation.

For more information on the general technique:

- https://en.wikipedia.org/wiki/Software_transactional_memory
- https://en.wikipedia.org/wiki/Compare-and-swap
- https://en.wikipedia.org/wiki/Copy-on-write
- https://en.wikipedia.org/wiki/Persistent_data_structure

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace OptimisticImmutableCollections
{
    public sealed class OptimisticSet<T> : OptimisticCollection<ImmutableHashSet<T>>, ISet<T>, IReadOnlyCollection<T>, ICollection
    {
        public OptimisticSet() : base(ImmutableHashSet<T>.Empty) { }

        public OptimisticSet(ImmutableHashSet<T> initial) : base(initial) { }

        public OptimisticSet(IEqualityComparer<T> equalityComparer) : base(ImmutableHashSet<T>.Empty.WithComparer(equalityComparer)) { }

        public int Count => Inner.Count;

        public bool IsEmpty => Inner.IsEmpty;

        public IEqualityComparer<T> KeyComparer => Inner.KeyComparer;

        public ImmutableHashSet<T>.Enumerator GetEnumerator() => Inner.GetEnumerator();

        public bool TryGetValue(T equalValue, out T actualValue) => Inner.TryGetValue(equalValue, out actualValue);

        public ImmutableHashSet<T>.Builder ToBuilder() => Inner.ToBuilder();

        public bool Add(T item) => TryUpdate(item, (set, i) => set.Add(i));

        public void Clear() => TryUpdate(set => set.Clear());

        public bool Remove(T item) => TryUpdate(item, (set, i) => set.Remove(i));

        public bool Contains(T item) => Inner.Contains(item);

        #region IEnumerable

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Inner.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Inner).GetEnumerator();

        #endregion

        #region ICollection

        bool ICollection<T>.IsReadOnly => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => throw new NotImplementedException();

        public void CopyTo(Array array, int index) => ((ICollection) Inner).CopyTo(array, index);

        public void CopyTo(T[] array, int arrayIndex) => ((ICollection<T>) Inner).CopyTo(array, arrayIndex);

        void ICollection<T>.Add(T item) => Add(item);

        #endregion

        #region Set operations

        public bool SetEquals(IEnumerable<T> other) => Inner.SetEquals(other);

        public bool IsProperSubsetOf(IEnumerable<T> other) => Inner.IsProperSubsetOf(other);

        public bool IsProperSupersetOf(IEnumerable<T> other) => Inner.IsProperSupersetOf(other);

        public bool IsSubsetOf(IEnumerable<T> other) => Inner.IsSubsetOf(other);

        public bool IsSupersetOf(IEnumerable<T> other) => Inner.IsSupersetOf(other);

        public bool Overlaps(IEnumerable<T> other) => Inner.Overlaps(other);

        public void ExceptWith(IEnumerable<T> other) => TryUpdate(other, (set, o) => set.Except(o));

        public void SymmetricExceptWith(IEnumerable<T> other) => TryUpdate(other, (set, o) => set.SymmetricExcept(o));

        public void IntersectWith(IEnumerable<T> other) => TryUpdate(other, (set, o) => set.Intersect(o));

        public void UnionWith(IEnumerable<T> other) => TryUpdate(other, (set, o) => set.Union(o));

        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace OptimisticImmutableCollections
{
    public sealed class OptimisticList<T> : OptimisticCollection<ImmutableList<T>>, ICollection<T>, IReadOnlyList<T>, ICollection
    {
        public OptimisticList() : base(ImmutableList<T>.Empty) { }

        public OptimisticList(ImmutableList<T> initial) : base(initial) { }

        public int Count => Inner.Count;

        public bool IsEmpty => Inner.IsEmpty;

        public ImmutableList<T>.Enumerator GetEnumerator() => Inner.GetEnumerator();

        public ImmutableList<T>.Builder ToBuilder() => Inner.ToBuilder();

        public bool Add(T item) => TryUpdate(item, (set, i) => set.Add(i));

        public void Clear() => TryUpdate(set => set.Clear());

        public bool Remove(T item) => TryUpdate(item, (set, i) => set.Remove(i));

        public bool Contains(T item) => Inner.Contains(item);

        public T this[int index] => Inner[index];

        #region IEnumerable

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Inner.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Inner).GetEnumerator();

        #endregion

        #region ICollection

        bool ICollection<T>.IsReadOnly => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => throw new NotImplementedException();

        public void CopyTo(Array array, int index) => ((ICollection)Inner).CopyTo(array, index);

        public void CopyTo(T[] array, int arrayIndex) => ((ICollection<T>)Inner).CopyTo(array, arrayIndex);

        void ICollection<T>.Add(T item) => Add(item);

        #endregion
    }
}

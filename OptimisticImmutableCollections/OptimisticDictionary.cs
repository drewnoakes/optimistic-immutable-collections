using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace OptimisticImmutableCollections
{
    public sealed class OptimisticDictionary<TKey, TValue> : OptimisticCollection<ImmutableDictionary<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, IDictionary<TKey, TValue>, ICollection
    {
        public OptimisticDictionary() : base(ImmutableDictionary<TKey, TValue>.Empty) { }

        public OptimisticDictionary(ImmutableDictionary<TKey, TValue> initial) : base(initial) { }

        public OptimisticDictionary(IEqualityComparer<TKey> keyComparer) : base(ImmutableDictionary<TKey, TValue>.Empty.WithComparers(keyComparer)) { }

        public OptimisticDictionary(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) : base(ImmutableDictionary<TKey, TValue>.Empty.WithComparers(keyComparer, valueComparer)) { }

        public int Count => Inner.Count;

        public bool IsEmpty => Inner.IsEmpty;

        // TODO could cache comparers as we don't allow them to change

        public IEqualityComparer<TKey> KeyComparer => Inner.KeyComparer;

        public IEqualityComparer<TValue> ValueComparer => Inner.ValueComparer;

        public ImmutableDictionary<TKey, TValue>.Enumerator GetEnumerator() => Inner.GetEnumerator();

        public bool TryGetValue(TKey key, out TValue value) => Inner.TryGetValue(key, out value);

        public ImmutableDictionary<TKey, TValue>.Builder ToBuilder() => Inner.ToBuilder();

        public bool TryAdd(TKey key, TValue value) => TryUpdate(key, value, (dictionary, k, v) => dictionary.Add(k, v));

        public void Add(TKey key, TValue value) => TryUpdate(key, value, (dictionary, k, v) => dictionary.Add(k, v));

        public void Clear() => TryUpdate(dictionary => dictionary.Clear());

        public bool Remove(TKey key) => TryUpdate(key, (dictionary, k) => dictionary.Remove(k));

        public bool Remove(KeyValuePair<TKey, TValue> pair) => TryUpdate(pair, (dictionary, p) => dictionary.Remove(p.Key)); // NOTE doesn't compare value

        public bool ContainsKey(TKey key) => Inner.ContainsKey(key);

        public TValue this[TKey key]
        {
            get => Inner[key];
            set => TryUpdate(key, value, (dictionary, k, v) => dictionary.SetItem(k, v));
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Inner.Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Inner.Values;

        public bool Contains(KeyValuePair<TKey, TValue> pair) => Inner.TryGetValue(pair.Key, out var value) && ValueComparer.Equals(value, pair.Value);

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Inner.Keys.ToList();

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Inner.Values.ToList();

        #region IEnumerable

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => Inner.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Inner).GetEnumerator();

        #endregion

        #region ICollection

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => throw new NotImplementedException();

        public void CopyTo(Array array, int index) => ((ICollection) Inner).CopyTo(array, index);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((ICollection<KeyValuePair<TKey, TValue>>) Inner).CopyTo(array, arrayIndex);

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair) => Add(pair.Key, pair.Value);

        #endregion
    }
}

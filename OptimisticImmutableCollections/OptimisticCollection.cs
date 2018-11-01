using System;
using System.Threading;

namespace OptimisticImmutableCollections
{
    public abstract class OptimisticCollection<T> where T : class
    {
        private T _inner;

        protected OptimisticCollection(T initial) => _inner = initial ?? throw new ArgumentNullException(nameof(initial));

        public T Inner => Volatile.Read(ref _inner);

        protected bool TryUpdate(Func<T, T> updateFunc)
        {
            while (true)
            {
                var prior = Volatile.Read(ref _inner);
                var next = updateFunc(prior);

                if (ReferenceEquals(prior, next))
                    return false;

                var original = Interlocked.CompareExchange(ref _inner, next, prior);

                if (ReferenceEquals(prior, original))
                    return true;
            }
        }

        protected bool TryUpdate<TItem>(TItem item, Func<T, TItem, T> updateFunc)
        {
            while (true)
            {
                var prior = Volatile.Read(ref _inner);
                var next = updateFunc(prior, item);

                if (ReferenceEquals(prior, next))
                    return false;

                var original = Interlocked.CompareExchange(ref _inner, next, prior);

                if (ReferenceEquals(prior, original))
                    return true;
            }
        }

        protected bool TryUpdate<T1, T2>(T1 item1, T2 item2, Func<T, T1, T2, T> updateFunc)
        {
            while (true)
            {
                var prior = Volatile.Read(ref _inner);
                var next = updateFunc(prior, item1, item2);

                if (ReferenceEquals(prior, next))
                    return false;

                var original = Interlocked.CompareExchange(ref _inner, next, prior);

                if (ReferenceEquals(prior, original))
                    return true;
            }
        }
    }
}

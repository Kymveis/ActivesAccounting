using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Instantiating.Implementation;

internal abstract class ContainerBase<T> : IContainer<T> where T : IUniqueItem
{
    private sealed class Comparer : Comparer<T>
    {
        private const int DEFAULT_RESULT = -1;
        private readonly Func<T, IEnumerable<IComparable>> _comparedPropertyGetters;

        public Comparer(Func<T, IEnumerable<IComparable>> aComparedPropertyGetters)
        {
            _comparedPropertyGetters = aComparedPropertyGetters;
        }

        public override int Compare(T? aLeft, T? aRight)
        {
            if (aLeft is null)
            {
                throw new ArgumentNullException(nameof(aLeft));
            }

            if (aRight is null)
            {
                throw new ArgumentNullException(nameof(aRight));
            }

            return _comparedPropertyGetters(aLeft)
                .Zip(_comparedPropertyGetters(aRight))
                .Select(aTuple => aTuple.First.CompareTo(aTuple.Second))
                .FirstOrDefault(aR => aR is not 0, DEFAULT_RESULT);
        }
    }

    private readonly SortedSet<T> _items;

    protected ContainerBase()
    {
        _items = new SortedSet<T>(new Comparer(GetComparableProperties));
    }

    protected abstract string ItemName { get; }
    protected abstract IEnumerable<IComparable> GetComparableProperties(T aItem);

    public void Add(T aItem)
    {
        if (!_items.Add(aItem))
        {
            throw Exceptions.AlreadyHasItem(ItemName, aItem.Guid, "Guid");
        }
    }

    public bool HasDuplicate(T aItem) => _items.Contains(aItem);

    public void Remove(T aItem)
    {
        if (!_items.Remove(aItem))
        {
            throw Exceptions.NotHasItem(ItemName, aItem.Guid, "Guid");
        }
    }

    void IContainer<T>.Clear() => _items.Clear();

    T IContainer<T>.Get(Guid aGuid) =>
        _items.FirstOrDefault(aI => aI.Guid.Equals(aGuid))
        ?? throw Exceptions.NotHasItem(ItemName, aGuid, "Guid");

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
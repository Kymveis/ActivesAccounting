using System;
using System.Collections.Generic;
using System.Linq;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Instantiating.Implementation;

internal abstract class ContainerBase<T> : IContainer where T : IUniqueItem
{
    private readonly Dictionary<Guid, T> _itemsByGuid = new();

    protected abstract string ItemName { get; }
    protected IEnumerable<T> Items => _itemsByGuid.Values;

    public void Clear() => _itemsByGuid.Clear();

    protected T GetItemByGuid(Guid aGuid) =>
        _itemsByGuid.TryGetValue(aGuid, out var item)
            ? item
            : throw Exceptions.NotHasItem(ItemName, aGuid, "Guid");

    protected T AddItem(T aItem, Guid aGuid) =>
        _itemsByGuid.TryAdd(aGuid, aItem)
            ? aItem
            : throw Exceptions.AlreadyHasItem(ItemName, aGuid, "Guid");
}
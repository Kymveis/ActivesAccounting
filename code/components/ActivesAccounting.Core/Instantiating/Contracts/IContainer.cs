using System;
using System.Collections.Generic;

using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Contracts;

public interface IContainer<T> : IEnumerable<T> where T : IUniqueItem
{
    void Add(T aItem);
    bool HasDuplicate(T aItem);
    void Remove(T aItem);
    internal void Clear();
    internal T Get(Guid aGuid);
}
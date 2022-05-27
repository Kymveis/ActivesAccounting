using System;

using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Contracts;

public interface IContainer<T> where T : IUniqueItem
{
    void Remove(T aItem);
    void Clear();
    internal T Get(Guid aGuid);
}
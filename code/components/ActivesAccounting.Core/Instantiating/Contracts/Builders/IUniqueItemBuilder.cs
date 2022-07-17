using System;

namespace ActivesAccounting.Core.Instantiating.Contracts.Builders;

public interface IUniqueItemBuilder<out T> : IBuilder<T>
{
    internal void SetGuid(Guid aGuid);
}
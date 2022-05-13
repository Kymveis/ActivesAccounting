using System;

namespace ActivesAccounting.Core.Model.Contracts;

public interface IUniqueItem
{
    Guid Guid { get; }
}
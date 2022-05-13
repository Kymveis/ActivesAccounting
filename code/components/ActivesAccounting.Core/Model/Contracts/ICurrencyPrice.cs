using System;

using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.Core.Model.Contracts;

public interface ICurrencyPrice : IUniqueItem
{
    ICurrency Exchanged { get; }
    ICurrency Unit { get; }
    PriceType Type { get; }
    decimal Count { get; }
    DateTime DateTime { get; }

    void RecordExactCount(decimal aExactCount);
}
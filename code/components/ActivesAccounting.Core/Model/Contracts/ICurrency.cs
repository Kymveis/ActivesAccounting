using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.Core.Model.Contracts;

public interface ICurrency : IUniqueItem, INamedItem
{
    CurrencyType Type { get; }
}
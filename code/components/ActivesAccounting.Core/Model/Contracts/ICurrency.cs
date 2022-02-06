using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.Core.Model.Contracts
{
    public interface ICurrency : IUniqueItem
    {
        string Name { get; }
        CurrencyType Type { get; }
    }
}
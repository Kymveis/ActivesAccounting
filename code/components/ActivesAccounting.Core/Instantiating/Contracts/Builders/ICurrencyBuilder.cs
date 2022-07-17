using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.Core.Instantiating.Contracts.Builders;

public interface ICurrencyBuilder : INamedItemBuilder<ICurrency>, IUniqueItemBuilder<ICurrency>
{
    void SetType(CurrencyType aCurrencyType);
}
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.Model.Contracts;

public interface ICurrencyTemplate : ITemplate<ICurrency>
{
    string? Name { get; set; }
    CurrencyType? Type { get; set; }
}
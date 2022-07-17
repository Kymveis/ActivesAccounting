using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;

using FluentValidation;

namespace ActivesAccounting.Core.Instantiating.Implementation.Builders;

internal sealed class CurrencyBuilder : UniqueItemBuilderBase<ICurrency, CurrencyBuilder.Currency>, ICurrencyBuilder
{
    internal sealed class Currency : UniqueItemBase, ICurrency
    {
        public string Name { get; set; } = string.Empty;
        public CurrencyType Type { get; set; }
    }

    public CurrencyBuilder(IValidator<ICurrency> aValidator) : base(aValidator)
    {
    }

    public void SetName(string aName) => Instance.Name = aName;

    public void SetType(CurrencyType aCurrencyType) => Instance.Type = aCurrencyType;
}
using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;

using FluentValidation;

using NullGuard;

namespace ActivesAccounting.Core.Instantiating.Implementation.Builders;

internal sealed class ValueBuilder : BuilderBase<IValue, ValueBuilder.Value>, IValueBuilder
{
    [NullGuard(ValidationFlags.None)]
    internal sealed class Value : IValue
    {
        public ICurrency Currency { get; set; }
        public decimal Count { get; set; }
        public IPlatform Platform { get; set; }

        public bool Equals(IValue? aOther) =>
            aOther is not null && Count == aOther.Count && Currency.Guid.Equals(aOther.Currency.Guid);
    }

    public ValueBuilder(IValidator<IValue> aValidator) : base(aValidator)
    {
    }

    public void SetPlatform(IPlatform aPlatform) => Instance.Platform = aPlatform;

    public void SetCurrency(ICurrency aCurrency) => Instance.Currency = aCurrency;

    public void SetCount(decimal aCount) => Instance.Count = aCount;
}
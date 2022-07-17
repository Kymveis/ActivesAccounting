using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Contracts.Builders;

public interface IValueBuilder : IBuilder<IValue>
{
    void SetPlatform(IPlatform aPlatform);
    void SetCurrency(ICurrency aCurrency);
    void SetCount(decimal aCount);
}
using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Contracts;

public interface IValueFactory
{
    IValue CreateValue(IPlatform aPlatform, ICurrency aCurrency, decimal aCount);
}
using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Contracts
{
    public interface IValueFactory
    {
        ISimpleValue CreateSimpleValue(IPlatform aPlatform, ICurrency aCurrency, decimal aCount);
    }
}
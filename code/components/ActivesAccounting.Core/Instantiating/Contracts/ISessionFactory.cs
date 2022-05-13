using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Contracts
{
    public interface ISessionFactory
    {
        ISession CreateSession(
            IRecordsContainer aRecordsContainer,
            IPricesContainer aPricesContainer,
            ICurrenciesContainer aCurrenciesContainer,
            IPlatformsContainer aPlatformsContainer);
    }
}
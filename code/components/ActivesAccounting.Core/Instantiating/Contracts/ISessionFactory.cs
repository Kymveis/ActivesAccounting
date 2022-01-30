using System.Collections.Generic;

using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Contracts
{
    public interface ISessionFactory
    {
        ISession CreateSession(
            IEnumerable<IRecord> aRecords,
            IEnumerable<ICurrencyPrice> aPrices,
            IEnumerable<ICurrency> aCurrencies);
    }
}
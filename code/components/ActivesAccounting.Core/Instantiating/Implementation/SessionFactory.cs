using System.Collections.Generic;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Instantiating.Implementation
{
    internal sealed class SessionFactory : ISessionFactory
    {
        private sealed record Session(IEnumerable<IRecord> Records, IEnumerable<ICurrencyPrice> Prices,
            IEnumerable<ICurrency> Currencies) : ISession;

        public ISession CreateSession(
            IEnumerable<IRecord> aRecords,
            IEnumerable<ICurrencyPrice> aPrices,
            IEnumerable<ICurrency> aCurrencies) =>
            new Session(
                aRecords.ValidateNotNull(nameof(aRecords)),
                aPrices.ValidateNotNull(nameof(aPrices)),
                aCurrencies.ValidateNotNull(nameof(aCurrencies)));
    }
}
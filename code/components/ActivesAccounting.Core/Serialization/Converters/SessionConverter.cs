using System.Collections.Generic;
using System.Text.Json;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Serialization.Converters
{
    internal sealed class SessionConverter : ConverterBase<ISession>
    {
        private readonly IRecordsContainer _recordsContainer;
        private readonly IPricesContainer _pricesContainer;
        private readonly ICurrenciesContainer _currenciesContainer;
        private readonly IPlatformsContainer _platformsContainer;

        private static class Names
        {
            public const string PLATFORMS = "Platforms";
            public const string CURRENCIES = "Currencies";
            public const string PRICES = "Prices";
            public const string RECORDS = "Records";
        }

        private readonly ISessionFactory _sessionFactory;

        public SessionConverter(
            ISessionFactory aSessionFactory,
            IRecordsContainer aRecordsContainer,
            IPricesContainer aPricesContainer,
            ICurrenciesContainer aCurrenciesContainer,
            IPlatformsContainer aPlatformsContainer)
        {
            _sessionFactory = aSessionFactory;
            _recordsContainer = aRecordsContainer;
            _pricesContainer = aPricesContainer;
            _currenciesContainer = aCurrenciesContainer;
            _platformsContainer = aPlatformsContainer;
        }

        protected override void Write(SerializingSession aSession, ISession aValue)
        {
            aSession.WriteProperty(aValue.Platforms, Names.PLATFORMS);
            aSession.WriteProperty(aValue.Currencies, Names.CURRENCIES);
            aSession.WriteProperty(aValue.Prices, Names.PRICES);
            aSession.WriteProperty(aValue.Records, Names.RECORDS);
        }

        protected override ISession Read(ref Utf8JsonReader aReader, JsonSerializerOptions aOptions)
        {
            Read<IReadOnlyCollection<IPlatform>>(ref aReader, Names.PLATFORMS, aOptions);
            Read<IReadOnlyCollection<ICurrency>>(ref aReader, Names.CURRENCIES, aOptions);
            Read<IReadOnlyCollection<ICurrencyPrice>>(ref aReader, Names.PRICES, aOptions);
            Read<IReadOnlyCollection<IRecord>>(ref aReader, Names.RECORDS, aOptions);

            return _sessionFactory.CreateSession(
                _recordsContainer,
                _pricesContainer,
                _currenciesContainer,
                _platformsContainer);
        }
    }
}
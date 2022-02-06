using System.Collections.Generic;
using System.Text.Json;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Serialization.Converters
{
    internal sealed class SessionConverter : ConverterBase<ISession>
    {
        private static class Names
        {
            public const string PLATFORMS = "Platforms";
            public const string CURRENCIES = "Currencies";
            public const string PRICES = "Prices";
            public const string RECORDS = "Records";
        }

        private readonly ISessionFactory _sessionFactory;

        internal SessionConverter(ISessionFactory aSessionFactory) =>
            _sessionFactory = aSessionFactory.ValidateNotNull(nameof(aSessionFactory));

        protected override void Write(SerializingSession aSession, ISession aValue)
        {
            aSession.WriteProperty(aValue.Platforms, Names.PLATFORMS);
            aSession.WriteProperty(aValue.Currencies, Names.CURRENCIES);
            aSession.WriteProperty(aValue.Prices, Names.PRICES);
            aSession.WriteProperty(aValue.Records, Names.RECORDS);
        }

        protected override ISession Read(ref Utf8JsonReader aReader, JsonSerializerOptions aOptions)
        {
            var platforms = Read<IReadOnlyCollection<IPlatform>>(ref aReader, Names.PLATFORMS, aOptions);
            var currencies = Read<IReadOnlyCollection<ICurrency>>(ref aReader, Names.CURRENCIES, aOptions);
            var prices = Read<IReadOnlyCollection<ICurrencyPrice>>(ref aReader, Names.PRICES, aOptions);
            var records = Read<IReadOnlyCollection<IRecord>>(ref aReader, Names.RECORDS, aOptions);

            return _sessionFactory.CreateSession(records, prices, currencies, platforms);
        }
    }
}
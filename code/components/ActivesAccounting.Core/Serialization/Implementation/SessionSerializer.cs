using System.Data;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Serialization.Contracts;
using ActivesAccounting.Core.Serialization.Converters;

namespace ActivesAccounting.Core.Serialization.Implementation
{
    internal sealed class SessionSerializer : ISessionSerializer
    {
        private readonly JsonSerializerOptions _options;

        public SessionSerializer(
            ISessionFactory aSessionFactory,
            IValueFactory aValueFactory,
            IPricesContainer aPricesContainer,
            ICurrenciesContainer aCurrenciesContainer,
            IRecordsContainer aRecordsContainer) =>
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new SessionConverter(aSessionFactory),
                    new CurrencyConverter(aCurrenciesContainer),
                    new CurrencyPriceConverter(aCurrenciesContainer, aPricesContainer),
                    new RecordConverter(aRecordsContainer),
                    new ValueConverter<IValue>(aCurrenciesContainer, aValueFactory),
                    new ValueConverter<ISimpleValue>(aCurrenciesContainer, aValueFactory),
                    new ValueConverter<ICombinedValue>(aCurrenciesContainer, aValueFactory)
                }
            };

        public Task SerializeAsync(Stream aStream, ISession aSession, CancellationToken aCancellationToken) =>
            JsonSerializer.SerializeAsync(aStream, aSession, _options, aCancellationToken);

        public async Task<ISession> DeserializeAsync(Stream aStream, CancellationToken aCancellationToken) =>
            await JsonSerializer.DeserializeAsync<ISession>(aStream, _options, aCancellationToken)
            ?? throw new NoNullAllowedException("Deserialized session cannot be null");
    }
}
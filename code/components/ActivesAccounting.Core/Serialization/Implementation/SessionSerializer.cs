using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Serialization.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Serialization.Implementation
{
    internal sealed class SessionSerializer : ISessionSerializer
    {
        private readonly JsonSerializerOptions _options;

        public SessionSerializer(IEnumerable<JsonConverter> aJsonConverters)
        {
            _options = new JsonSerializerOptions {WriteIndented = true};
            aJsonConverters.ForEach(_options.Converters.Add);
        }

        public Task SerializeAsync(Stream aStream, ISession aSession, CancellationToken aCancellationToken) =>
            JsonSerializer.SerializeAsync(aStream, aSession, _options, aCancellationToken);

        public async Task<ISession> DeserializeAsync(Stream aStream, CancellationToken aCancellationToken) =>
            await JsonSerializer.DeserializeAsync<ISession>(aStream, _options, aCancellationToken)
            ?? throw new NoNullAllowedException("Deserialized session cannot be null");
    }
}
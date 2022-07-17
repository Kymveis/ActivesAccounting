using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Implementation;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Serialization.Converters;

internal sealed class SessionConverter : ConverterBase<ISession>
{
    private static class Names
    {
        public const string PLATFORMS = "Platforms";
        public const string CURRENCIES = "Currencies";
        public const string RECORDS = "Records";
    }

    private readonly ISessionFactory _sessionFactory;

    public SessionConverter(ISessionFactory aSessionFactory)
    {
        _sessionFactory = aSessionFactory;
    }

    protected override void Write(SerializingSession aSession, ISession aValue)
    {
        aSession.WriteProperty(aValue.Platforms, Names.PLATFORMS);
        aSession.WriteProperty(aValue.Currencies, Names.CURRENCIES);
        aSession.WriteProperty(aValue.Records, Names.RECORDS);
    }

    protected override IResult<ISession> Read(ref Utf8JsonReader aReader, JsonSerializerOptions aOptions)
    {
        var session = _sessionFactory.Create();

        aOptions.Converters
            .OfType<ValueConverter>()
            .Single()
            .SetContainers(session.Platforms, session.Currencies);

        Read<IEnumerable<IPlatform>>(ref aReader, Names.PLATFORMS, aOptions)
            .ForEach(session.Platforms.Add);

        Read<IEnumerable<ICurrency>>(ref aReader, Names.CURRENCIES, aOptions)
            .ForEach(session.Currencies.Add);

        Read<IEnumerable<IRecord>>(ref aReader, Names.RECORDS, aOptions)
            .ForEach(session.Records.Add);

        return new Result<ISession>(session);
    }
}
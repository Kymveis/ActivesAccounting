using System.Text.Json;

using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Serialization.Converters;

internal sealed class PlatformConverter : ConverterBase<IPlatform>
{
    private static class Names
    {
        public const string NAME = "Name";
    }

    private readonly IBuilderFactory<IPlatformBuilder> _platformBuilderFactory;

    public PlatformConverter(IBuilderFactory<IPlatformBuilder> aPlatformBuilderFactory)
    {
        _platformBuilderFactory = aPlatformBuilderFactory;
    }

    protected override void Write(SerializingSession aSession, IPlatform aValue)
    {
        aSession.WriteProperty(aValue.Name, Names.NAME);
        aSession.WriteGuid(aValue);
    }

    protected override IResult<IPlatform> Read(ref Utf8JsonReader aReader, JsonSerializerOptions aOptions)
    {
        var builder = _platformBuilderFactory.Create();

        builder.SetName(ReadString(ref aReader, Names.NAME));
        builder.SetGuid(ReadGuid(ref aReader));

        return builder.Build();
    }
}
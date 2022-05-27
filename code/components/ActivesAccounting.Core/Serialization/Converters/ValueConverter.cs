using System;
using System.Text.Json;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Serialization.Converters;

internal sealed class ValueConverter : ConverterBase<IValue>
{
    private readonly IPlatformsContainer _platformsContainer;

    private static class Names
    {
        public const string PLATFORM = "Platform";
        public const string CURRENCY = "Currency";
        public const string COUNT = "Count";
    }

    private readonly ICurrenciesContainer _currenciesContainer;
    private readonly IValueFactory _valueFactory;

    public ValueConverter(
        ICurrenciesContainer aCurrenciesContainer,
        IPlatformsContainer aPlatformsContainer,
        IValueFactory aValueFactory)
    {
        _currenciesContainer = aCurrenciesContainer;
        _platformsContainer = aPlatformsContainer;
        _valueFactory = aValueFactory;
    }

    protected override void Write(SerializingSession aSession, IValue aValue)
    {
        aSession.WriteGuidRef(aValue.Platform, Names.PLATFORM);
        aSession.WriteGuidRef(aValue.Currency, Names.CURRENCY);
        aSession.WriteProperty(aValue.Count, Names.COUNT);
    }

    protected override IValue Read(ref Utf8JsonReader aReader, JsonSerializerOptions aOptions)
    {
        var platform = _platformsContainer.Get(ReadGuid(ref aReader, Names.PLATFORM));
        var currency = _currenciesContainer.Get(ReadGuid(ref aReader, Names.CURRENCY));
        var count = ReadDecimal(ref aReader, Names.COUNT);

        return _valueFactory.CreateValue(platform, currency, count);
    }
}
using System;
using System.Text.Json;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Serialization.Converters;

internal sealed class ValueConverter : ConverterBase<IValue>
{
    private static class Names
    {
        public const string PLATFORM = "Platform";
        public const string CURRENCY = "Currency";
        public const string COUNT = "Count";
    }

    private readonly IBuilderFactory<IValueBuilder> _valueBuilderFactory;
    private IContainer<IPlatform>? _platformsContainer;
    private IContainer<ICurrency>? _currenciesContainer;

    public ValueConverter(IBuilderFactory<IValueBuilder> aValueBuilderFactory)
    {
        _valueBuilderFactory = aValueBuilderFactory;
    }

    public void SetContainers(IContainer<IPlatform> aPlatformsContainer, IContainer<ICurrency>? aCurrenciesContainer)
    {
        _platformsContainer = aPlatformsContainer;
        _currenciesContainer = aCurrenciesContainer;
    }

    protected override void Write(SerializingSession aSession, IValue aValue)
    {
        aSession.WriteGuidRef(aValue.Platform, Names.PLATFORM);
        aSession.WriteGuidRef(aValue.Currency, Names.CURRENCY);
        aSession.WriteProperty(aValue.Count, Names.COUNT);
    }

    protected override IResult<IValue> Read(ref Utf8JsonReader aReader, JsonSerializerOptions aOptions)
    {
        if (_platformsContainer is null || _currenciesContainer is null)
        {
            throw new InvalidOperationException("Containers aren't set.");
        }

        var builder = _valueBuilderFactory.Create();

        builder.SetPlatform(_platformsContainer.Get(ReadGuid(ref aReader, Names.PLATFORM)));
        builder.SetCurrency(_currenciesContainer.Get(ReadGuid(ref aReader, Names.CURRENCY)));
        builder.SetCount(ReadDecimal(ref aReader, Names.COUNT));

        return builder.Build();
    }
}
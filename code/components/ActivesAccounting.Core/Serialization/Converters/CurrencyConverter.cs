using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;

using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.Core.Serialization.Converters;

internal sealed class CurrencyConverter : ConverterBase<ICurrency>
{
    private static class CurrencyTypes
    {
        private const string FIAT = "Fiat";
        private const string CRYPTO = "Crypto";
        private const string STOCK = "Stock";

        public static readonly IReadOnlyDictionary<string, CurrencyType> FromName =
            new Dictionary<string, CurrencyType>
            {
                {FIAT, CurrencyType.Fiat},
                {CRYPTO, CurrencyType.Crypto},
                {STOCK, CurrencyType.Stock}
            }.ToImmutableDictionary();

        public static readonly IReadOnlyDictionary<CurrencyType, string> ToName =
            new Dictionary<CurrencyType, string>
            {
                {CurrencyType.Fiat, FIAT},
                {CurrencyType.Crypto, CRYPTO},
                {CurrencyType.Stock, STOCK}
            }.ToImmutableDictionary();
    }

    private static class Names
    {
        public const string NAME = "Name";
        public const string TYPE = "Type";
    }

    private readonly IBuilderFactory<ICurrencyBuilder> _currencyBuilderFactory;

    public CurrencyConverter(IBuilderFactory<ICurrencyBuilder> aCurrencyBuilderFactory)
    {
        _currencyBuilderFactory = aCurrencyBuilderFactory;
    }

    protected override void Write(SerializingSession aSession, ICurrency aValue)
    {
        aSession.WriteProperty(aValue.Name, Names.NAME);
        aSession.WriteProperty(CurrencyTypes.ToName[aValue.Type], Names.TYPE);
        aSession.WriteGuid(aValue);
    }

    protected override IResult<ICurrency> Read(ref Utf8JsonReader aReader, JsonSerializerOptions aOptions)
    {
        var builder = _currencyBuilderFactory.Create();

        builder.SetName(ReadString(ref aReader, Names.NAME));
        builder.SetType(CurrencyTypes.FromName[ReadString(ref aReader, Names.TYPE)]);
        builder.SetGuid(ReadGuid(ref aReader));

        return builder.Build();
    }
}
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Serialization.Converters
{
    internal sealed class CurrencyPriceConverter : ConverterBase<ICurrencyPrice>
    {
        private static class PriceTypes
        {
            private const string RECORDED_TYPE = "Recorded";
            private const string CALCULATED_TYPE = "Calculated";

            public static readonly IReadOnlyDictionary<string, PriceType> FromName =
                new Dictionary<string, PriceType>
                {
                    {RECORDED_TYPE, PriceType.Recorded},
                    {CALCULATED_TYPE, PriceType.Calculated}
                }.ToImmutableDictionary();

            public static readonly IReadOnlyDictionary<PriceType, string> ToName =
                new Dictionary<PriceType, string>
                {
                    {PriceType.Recorded, RECORDED_TYPE},
                    {PriceType.Calculated, CALCULATED_TYPE}
                }.ToImmutableDictionary();
        }

        private static class Names
        {
            public const string DATE_TIME = "DateTime";
            public const string PRICE_TYPE = "PriceType";
            public const string UNIT = "Unit";
            public const string EXCHANGED = "Exchanged";
            public const string COUNT = "Count";
        }

        private readonly ICurrenciesContainer _currenciesContainer;
        private readonly IPricesContainer _pricesContainer;

        public CurrencyPriceConverter(
            ICurrenciesContainer aCurrenciesContainer,
            IPricesContainer aPricesContainer)
        {
            _currenciesContainer = aCurrenciesContainer.ValidateNotNull(nameof(aCurrenciesContainer));
            _pricesContainer = aPricesContainer.ValidateNotNull(nameof(aPricesContainer));
        }

        protected override void Write(SerializingSession aSession, ICurrencyPrice aValue)
        {
            aSession.WriteProperty(aValue.DateTime, Names.DATE_TIME);
            aSession.WriteProperty(PriceTypes.ToName[aValue.Type], Names.PRICE_TYPE);
            aSession.WriteGuidRef(aValue.Unit, Names.UNIT);
            aSession.WriteGuidRef(aValue.Exchanged, Names.EXCHANGED);
            aSession.WriteProperty(aValue.Count, Names.COUNT);
            aSession.WriteGuid(aValue);
        }

        protected override ICurrencyPrice Read(ref Utf8JsonReader aReader, JsonSerializerOptions aOptions)
        {
            var dateTime = ReadDateTime(ref aReader, Names.DATE_TIME);
            var priceType = PriceTypes.FromName[ReadString(ref aReader, Names.PRICE_TYPE)];
            var unit = _currenciesContainer.GetCurrency(ReadGuid(ref aReader, Names.UNIT));
            var exchanged = _currenciesContainer.GetCurrency(ReadGuid(ref aReader, Names.EXCHANGED));
            var count = ReadDecimal(ref aReader, Names.COUNT);
            var guid = ReadGuid(ref aReader);

            return _pricesContainer.CreatePrice(
                exchanged,
                unit,
                count,
                priceType,
                dateTime,
                guid);
        }
    }
}
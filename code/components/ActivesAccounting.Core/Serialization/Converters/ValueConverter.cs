using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

using ValueType = ActivesAccounting.Core.Model.Enums.ValueType;

namespace ActivesAccounting.Core.Serialization.Converters
{
    internal sealed class ValueConverter<T> : ConverterBase<T> where T : class, IValue
    {
        private static class ValueTypes
        {
            private const string SIMPLE_TYPE = "Simple";
            private const string COMBINED_TYPE = "Combined";

            public static readonly IReadOnlyDictionary<string, ValueType> FromName =
                new Dictionary<string, ValueType>
                {
                    {SIMPLE_TYPE, ValueType.Simple},
                    {COMBINED_TYPE, ValueType.Combined}
                }.ToImmutableDictionary();

            public static readonly IReadOnlyDictionary<ValueType, string> ToName =
                new Dictionary<ValueType, string>
                {
                    {ValueType.Simple, SIMPLE_TYPE},
                    {ValueType.Combined, COMBINED_TYPE}
                }.ToImmutableDictionary();
        }

        private static class Names
        {
            public const string VALUE_TYPE = "ValueType";
            public const string CURRENCY = "Currency";
            public const string COUNT = "Count";
            public const string VALUES = "Values";
        }

        private readonly ICurrenciesContainer _currenciesContainer;
        private readonly IValueFactory _valueFactory;

        public ValueConverter(
            ICurrenciesContainer aCurrenciesContainer,
            IValueFactory aValueFactory)
        {
            _currenciesContainer = aCurrenciesContainer.ValidateNotNull(nameof(aCurrenciesContainer));
            _valueFactory = aValueFactory.ValidateNotNull(nameof(aValueFactory));
        }

        protected override void Write(SerializingSession aSession, T aValue)
        {
            aSession.WriteProperty(ValueTypes.ToName[aValue.ValueType], Names.VALUE_TYPE);

            switch (aValue)
            {
                case ISimpleValue simpleValue when aValue.ValueType is ValueType.Simple:
                    aSession.WriteGuidRef(simpleValue.Currency, Names.CURRENCY);
                    aSession.WriteProperty(simpleValue.Count, Names.COUNT);
                    break;
                case ICombinedValue combinedValue when aValue.ValueType is ValueType.Combined:
                    aSession.WriteProperty((IEnumerable<ISimpleValue>) combinedValue, Names.VALUES);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(aValue), aValue, default);
            }
        }

        protected override T Read(ref Utf8JsonReader aReader, JsonSerializerOptions aOptions)
        {
            var type = ValueTypes.FromName[ReadString(ref aReader, Names.VALUE_TYPE)];

            switch (type)
            {
                case ValueType.Simple:
                    var currency = _currenciesContainer.GetCurrency(ReadGuid(ref aReader, Names.CURRENCY));
                    var count = ReadDecimal(ref aReader, Names.COUNT);
                    return (T) _valueFactory.CreateSimpleValue(currency, count);
                case ValueType.Combined:
                    return (T) _valueFactory.CreateCombinedValue(
                        Read<IReadOnlyCollection<ISimpleValue>>(ref aReader, Names.VALUES, aOptions));
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, default);
            }
        }
    }
}
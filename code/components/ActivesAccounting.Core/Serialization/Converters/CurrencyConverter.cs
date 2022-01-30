using System.Data;
using System.Text.Json;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Serialization.Converters
{
    internal sealed class CurrencyConverter : ConverterBase<ICurrency>
    {
        private static class Names
        {
            public const string NAME = "Name";
        }
        
        private readonly ICurrenciesContainer _currenciesContainer;

        public CurrencyConverter(ICurrenciesContainer aCurrenciesContainer) => _currenciesContainer =
            aCurrenciesContainer.ValidateNotNull(nameof(aCurrenciesContainer));

        protected override void Write(SerializingSession aSession, ICurrency aValue)
        {
            aSession.WriteProperty(aValue.Name, Names.NAME);
            aSession.WriteGuid(aValue);
        }

        protected override ICurrency Read(ref Utf8JsonReader aReader, JsonSerializerOptions aOptions)
        {
            var name = ReadString(ref aReader, Names.NAME);
            var guid = ReadGuid(ref aReader);
            return _currenciesContainer.CreateCurrency(name, guid);
        }
    }
}
using System.Text.Json;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Serialization.Converters
{
    internal sealed class PlatformConverter : ConverterBase<IPlatform>
    {
        private static class Names
        {
            public const string NAME = "Name";
        }

        private readonly IPlatformsContainer _platformsContainer;

        public PlatformConverter(IPlatformsContainer aPlatformsContainer)
        {
            _platformsContainer = aPlatformsContainer.ValidateNotNull(nameof(aPlatformsContainer));
        }

        protected override void Write(SerializingSession aSession, IPlatform aValue)
        {
            aSession.WriteProperty(aValue.Name, Names.NAME);
            aSession.WriteGuid(aValue);
        }

        protected override IPlatform Read(ref Utf8JsonReader aReader, JsonSerializerOptions aOptions)
        {
            var name = ReadString(ref aReader, Names.NAME);
            var guid = ReadGuid(ref aReader);

            return _platformsContainer.CreatePlatform(name, guid);
        }
    }
}
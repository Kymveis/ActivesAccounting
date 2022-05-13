using System;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Serialization.Converters
{
    internal abstract class ConverterBase<T> : JsonConverter<T>
    {
        private const string GUID_NAME = "Guid";

        protected sealed class SerializingSession
        {
            private readonly Utf8JsonWriter _writer;
            private readonly JsonSerializerOptions _options;

            public SerializingSession(Utf8JsonWriter aWriter, JsonSerializerOptions aOptions)
            {
                _writer = aWriter;
                _options = aOptions;
            }

            public void WriteProperty<TProperty>(TProperty aValue, string aName)
            {
                _writer.WritePropertyName(aName);
                JsonSerializer.Serialize(_writer, aValue, _options);
            }

            public void WriteGuid(IUniqueItem aUniqueItem) => WriteProperty(aUniqueItem.Guid, GUID_NAME);

            public void WriteGuidRef(IUniqueItem aUniqueItem, string aPropertyName) =>
                WriteProperty(aUniqueItem.Guid, aPropertyName + GUID_NAME);
        }

        protected abstract void Write(SerializingSession aSession, T aValue);
        protected abstract T Read(ref Utf8JsonReader aReader, JsonSerializerOptions aOptions);

        public override void Write(Utf8JsonWriter aWriter, T aValue, JsonSerializerOptions aOptions)
        {
            aWriter.WriteStartObject();
            Write(new SerializingSession(aWriter, aOptions), aValue);
            aWriter.WriteEndObject();
        }

        public override T Read(ref Utf8JsonReader aReader, Type aTypeToConvert, JsonSerializerOptions aOptions)
        {
            if (typeof(T) != aTypeToConvert)
            {
                throw new InvalidOperationException(
                    $"The given type is {aTypeToConvert} when expected was {typeof(T)}.");
            }

            var read = Read(ref aReader, aOptions);
            moveReader(ref aReader);
            return read;
        }

        protected string ReadString(ref Utf8JsonReader aReader, string aPropertyName)
        {
            validatePropertyName(ref aReader, aPropertyName);
            return aReader.GetString() ?? throw createReadNullException();
        }

        protected decimal ReadDecimal(ref Utf8JsonReader aReader, string aPropertyName)
        {
            validatePropertyName(ref aReader, aPropertyName);
            return aReader.GetDecimal();
        }

        protected DateTime ReadDateTime(ref Utf8JsonReader aReader, string aPropertyName)
        {
            validatePropertyName(ref aReader, aPropertyName);
            return aReader.GetDateTime();
        }

        protected Guid ReadGuid(ref Utf8JsonReader aReader, string aNamePrefix = "")
        {
            validatePropertyName(ref aReader, aNamePrefix + GUID_NAME);
            return aReader.GetGuid();
        }

        protected TValue Read<TValue>(ref Utf8JsonReader aReader, string aPropertyName, JsonSerializerOptions aOptions, bool aAllowNull = false)
        {
            validatePropertyName(ref aReader, aPropertyName);
            return JsonSerializer.Deserialize<TValue>(ref aReader, aOptions) ?? (aAllowNull ? default! : throw createReadNullException());
        }

        private static void validatePropertyName(ref Utf8JsonReader aReader, string aPropertyName)
        {
            moveReader(ref aReader);
            if (!aReader.ValueTextEquals(aPropertyName))
            {
                throw new InvalidDataException(
                    $"Unexpected property: {aReader.GetString()} instead of {aPropertyName}");
            }

            moveReader(ref aReader);
        }

        private static void moveReader(ref Utf8JsonReader aReader)
        {
            if (!aReader.Read())
            {
                throw new InvalidDataException("JSON stream is not enough.");
            }
        }

        private static Exception createReadNullException() => new NoNullAllowedException();
    }
}
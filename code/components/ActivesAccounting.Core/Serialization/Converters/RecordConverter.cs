using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Serialization.Converters
{
    internal sealed class RecordConverter : ConverterBase<IRecord>
    {
        private static class RecordTypes
        {
            private const string DEPOSIT = "Deposit";
            private const string TRANSFER = "Transfer";
            private const string WITHDRAWAL = "Withdrawal";

            public static readonly IReadOnlyDictionary<string, RecordType> FromName = new Dictionary<string, RecordType>
            {
                {DEPOSIT, RecordType.Deposit},
                {TRANSFER, RecordType.Transfer},
                {WITHDRAWAL, RecordType.Withdrawal}
            }.ToImmutableDictionary();

            public static readonly IReadOnlyDictionary<RecordType, string> ToName = new Dictionary<RecordType, string>
            {
                {RecordType.Deposit, DEPOSIT},
                {RecordType.Transfer, TRANSFER},
                {RecordType.Withdrawal, WITHDRAWAL}
            }.ToImmutableDictionary();
        }

        private static class Names
        {
            public const string DATE_TIME = "DateTime";
            public const string RECORD_TYPE = "RecordType";
            public const string SOURCE = "Source";
            public const string TARGET = "Target";
            public const string COMMISSION = "Commission";
        }

        private readonly IRecordsContainer _recordsContainer;

        public RecordConverter(IRecordsContainer aRecordsContainer) =>
            _recordsContainer = aRecordsContainer;

        protected override void Write(SerializingSession aSession, IRecord aValue)
        {
            aSession.WriteProperty(aValue.DateTime, Names.DATE_TIME);
            aSession.WriteProperty(RecordTypes.ToName[aValue.RecordType], Names.RECORD_TYPE);
            aSession.WriteProperty(aValue.Source, Names.SOURCE);
            aSession.WriteProperty(aValue.Target, Names.TARGET);
            aSession.WriteProperty(aValue.Commission, Names.COMMISSION);
            aSession.WriteGuid(aValue);
        }

        protected override IRecord Read(ref Utf8JsonReader aReader, JsonSerializerOptions aOptions)
        {
            var dateTime = ReadDateTime(ref aReader, Names.DATE_TIME);
            var recordType = RecordTypes.FromName[ReadString(ref aReader, Names.RECORD_TYPE)];
            var source = Read<IValue>(ref aReader, Names.SOURCE, aOptions);
            var target = Read<IValue>(ref aReader, Names.TARGET, aOptions);
            var commission = Read<IValue>(ref aReader, Names.COMMISSION, aOptions, true);
            var guid = ReadGuid(ref aReader);

            return _recordsContainer.CreateRecord(
                dateTime,
                recordType,
                source,
                target,
                commission,
                guid);
        }
    }
}
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;

using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.Core.Serialization.Converters;

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
    }

    private readonly IBuilderFactory<IRecordBuilder> _recordBuilderFactory;

    public RecordConverter(IBuilderFactory<IRecordBuilder> aRecordBuilderFactory)
    {
        _recordBuilderFactory = aRecordBuilderFactory;
    }

    protected override void Write(SerializingSession aSession, IRecord aValue)
    {
        aSession.WriteProperty(aValue.DateTime, Names.DATE_TIME);
        aSession.WriteProperty(RecordTypes.ToName[aValue.RecordType], Names.RECORD_TYPE);
        aSession.WriteProperty(aValue.Source, Names.SOURCE);
        aSession.WriteProperty(aValue.Target, Names.TARGET);
        aSession.WriteGuid(aValue);
    }

    protected override IResult<IRecord> Read(ref Utf8JsonReader aReader, JsonSerializerOptions aOptions)
    {
        var builder = _recordBuilderFactory.Create();

        builder.SetDateTime(ReadDateTime(ref aReader, Names.DATE_TIME));
        builder.SetRecordType(RecordTypes.FromName[ReadString(ref aReader, Names.RECORD_TYPE)]);
        builder.SetSourceValue(Read<IValue>(ref aReader, Names.SOURCE, aOptions));
        builder.SetTargetValue(Read<IValue>(ref aReader, Names.TARGET, aOptions));
        builder.SetGuid(ReadGuid(ref aReader));

        return builder.Build();
    }
}
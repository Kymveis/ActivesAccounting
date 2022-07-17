using System;

using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;

using FluentValidation;

using NullGuard;

namespace ActivesAccounting.Core.Instantiating.Implementation.Builders;

internal sealed class RecordBuilder : UniqueItemBuilderBase<IRecord, RecordBuilder.Record>, IRecordBuilder
{
    [NullGuard(ValidationFlags.None)]
    internal sealed class Record : UniqueItemBase, IRecord
    {
        public DateTime DateTime { get; set; }
        public RecordType RecordType { get; set; }
        public IValue Source { get; set; }
        public IValue Target { get; set; }
    }

    public RecordBuilder(IValidator<IRecord> aValidator) : base(aValidator)
    {
    }

    public void SetDateTime(DateTime aDateTime) => Instance.DateTime = aDateTime;

    public void SetRecordType(RecordType aRecordType) => Instance.RecordType = aRecordType;

    public void SetSourceValue(IValue aSourceValue) => Instance.Source = aSourceValue;

    public void SetTargetValue(IValue aTargetValue) => Instance.Target = aTargetValue;
}
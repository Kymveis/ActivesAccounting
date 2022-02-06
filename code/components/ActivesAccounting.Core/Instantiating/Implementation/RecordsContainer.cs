using System;
using System.Collections.Generic;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Instantiating.Implementation
{
    internal sealed class RecordsContainer : ContainerBase<IRecord>, IRecordsContainer
    {
        private sealed record Record(DateTime DateTime, RecordType RecordType, IValue Source, IValue Target,
            Guid Guid) : IRecord;

        protected override string ItemName => "record";
        public IEnumerable<IRecord> Records => Items;

        public IRecord CreateRecord(
            DateTime aDateTime,
            RecordType aRecordType,
            IValue aSource,
            IValue aTarget) =>
            createRecord(aDateTime, aRecordType, aSource, aTarget, Guid.NewGuid());

        IRecord IRecordsContainer.CreateRecord(
            DateTime aDateTime,
            RecordType aRecordType,
            IValue aSource,
            IValue aTarget,
            Guid aGuid) =>
            createRecord(aDateTime, aRecordType, aSource, aTarget, aGuid);

        private IRecord createRecord(
            DateTime aDateTime,
            RecordType aRecordType,
            IValue aSource,
            IValue aTarget,
            Guid aGuid) =>
            AddItem(
                new Record(
                    aDateTime,
                    aRecordType.ValidateEnum(nameof(aRecordType), RecordType.Undefined),
                    aSource.ValidateNotNull(nameof(aSource)),
                    aTarget.ValidateNotNull(nameof(aTarget)),
                    aGuid),
                aGuid);
    }
}
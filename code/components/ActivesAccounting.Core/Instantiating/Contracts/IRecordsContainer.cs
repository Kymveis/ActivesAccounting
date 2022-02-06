using System;
using System.Collections.Generic;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.Core.Instantiating.Contracts
{
    public interface IRecordsContainer : IContainer
    {
        IEnumerable<IRecord> Records { get; }

        IRecord CreateRecord(
            DateTime aDateTime,
            RecordType aRecordType,
            IValue aSource,
            IValue aTarget);

        internal IRecord CreateRecord(
            DateTime aDateTime,
            RecordType aRecordType,
            IValue aSource,
            IValue aTarget,
            Guid aGuid);
    }
}
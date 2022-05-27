using System;
using System.Collections.Generic;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.Core.Instantiating.Contracts;

public interface IRecordsContainer : IContainer<IRecord>
{
    IReadOnlySet<IRecord> Records { get; }

    IRecord CreateRecord(
        DateTime aDateTime,
        RecordType aRecordType,
        IValue aSource,
        IValue aTarget,
        IValue? aCommission = null);

    internal IRecord CreateRecord(
        DateTime aDateTime,
        RecordType aRecordType,
        IValue aSource,
        IValue aTarget,
        IValue? aCommission,
        Guid aGuid);
}
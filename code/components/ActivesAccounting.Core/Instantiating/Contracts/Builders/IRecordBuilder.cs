using System;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.Core.Instantiating.Contracts.Builders;

public interface IRecordBuilder : IUniqueItemBuilder<IRecord>
{
    void SetDateTime(DateTime aDateTime);
    void SetRecordType(RecordType aRecordType);

    void SetSourceValue(IValue aSourceValue);
    void SetTargetValue(IValue aTargetValue);
}
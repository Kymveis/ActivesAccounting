using System.Collections.Generic;

using ValueType = ActivesAccounting.Core.Model.Enums.ValueType;

namespace ActivesAccounting.Core.Model.Contracts
{
    public interface ICombinedValue : IValue, IReadOnlyCollection<ISimpleValue>
    {
        ValueType IValue.ValueType => ValueType.Combined;
    }
}
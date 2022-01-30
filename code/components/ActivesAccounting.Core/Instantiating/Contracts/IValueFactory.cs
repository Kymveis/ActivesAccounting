using System.Collections.Generic;

using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Contracts
{
    public interface IValueFactory
    {
        ISimpleValue CreateSimpleValue(ICurrency aCurrency, decimal aCount);
        ICombinedValue CreateCombinedValue(IEnumerable<ISimpleValue> aSimpleValues);
    }
}
using System;
using System.Collections.Generic;

using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Contracts
{
    public interface ICurrenciesContainer : IContainer
    {
        IEnumerable<ICurrency> Currencies { get; }

        ICurrency CreateCurrency(string aName);

        internal ICurrency CreateCurrency(string aName, Guid aGuid);
        internal ICurrency GetCurrency(Guid aCurrencyGuid);
    }
}
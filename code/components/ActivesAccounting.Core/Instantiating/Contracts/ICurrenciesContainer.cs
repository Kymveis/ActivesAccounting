using System;
using System.Collections.Generic;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.Core.Instantiating.Contracts;

public interface ICurrenciesContainer : IContainer<ICurrency>
{
    IReadOnlySet<ICurrency> Currencies { get; }

    ICurrency CreateCurrency(string aName, CurrencyType aType);

    internal ICurrency CreateCurrency(string aName, CurrencyType aType, Guid aGuid);
}
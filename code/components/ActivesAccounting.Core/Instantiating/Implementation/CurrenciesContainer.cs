using System;
using System.Collections.Generic;

using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Implementation;

internal sealed class CurrenciesContainer : ContainerBase<ICurrency>
{
    protected override string ItemName => "currency";

    protected override IEnumerable<IComparable> GetComparableProperties(ICurrency aItem)
    {
        yield return aItem.Type;
        yield return aItem.Name;
    }
}
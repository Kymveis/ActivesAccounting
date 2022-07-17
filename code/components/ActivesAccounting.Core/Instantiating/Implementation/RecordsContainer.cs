using System;
using System.Collections.Generic;

using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Implementation;

internal sealed class RecordsContainer : ContainerBase<IRecord>
{
    protected override string ItemName => "record";

    protected override IEnumerable<IComparable> GetComparableProperties(IRecord aItem)
    {
        yield return aItem.DateTime;
        yield return aItem.RecordType;
        yield return aItem.Source.Platform.Name;
        yield return aItem.Source.Currency.Name;
        yield return aItem.Target.Platform.Name;
        yield return aItem.Target.Currency.Name;
    }
}
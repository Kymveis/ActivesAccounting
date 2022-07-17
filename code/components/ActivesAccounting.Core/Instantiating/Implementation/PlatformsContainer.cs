using System;
using System.Collections.Generic;

using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Implementation;

internal sealed class PlatformsContainer : ContainerBase<IPlatform>
{
    protected override string ItemName => "platform";

    protected override IEnumerable<IComparable> GetComparableProperties(IPlatform aItem)
    {
        yield return aItem.Name;
    }
}
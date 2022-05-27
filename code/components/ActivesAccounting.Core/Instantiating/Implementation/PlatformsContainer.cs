using System;
using System.Collections.Generic;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Instantiating.Implementation;

internal sealed class PlatformsContainer : NamedItemsContainerBase<IPlatform>, IPlatformsContainer
{
    private sealed record Platform(string Name, Guid Guid) : IPlatform;

    protected override string ItemName => "platform";
    protected override IEnumerable<IComparable> GetComparableProperties(IPlatform aItem)
    {
        yield return aItem.Name;
    }

    public IReadOnlySet<IPlatform> Platforms => Items;

    public IPlatform CreatePlatform(string aName) => createPlatform(aName, Guid.NewGuid());

    IPlatform IPlatformsContainer.CreatePlatform(string aName, Guid aGuid) => createPlatform(aName, aGuid);

    private IPlatform createPlatform(string aName, Guid aGuid)
    {
        ValidateUniqueName(aName.ValidateNotEmptyOrWhitespace());

        return Add(new Platform(aName, aGuid));
    }
}
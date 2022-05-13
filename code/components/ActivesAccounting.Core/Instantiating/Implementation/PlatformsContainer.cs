using System;
using System.Collections.Generic;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Instantiating.Implementation;

internal sealed class PlatformsContainer : ContainerBase<IPlatform>, IPlatformsContainer
{
    private sealed record Platform(string Name, Guid Guid) : IPlatform;

    protected override string ItemName => "platform";
    public IEnumerable<IPlatform> Platforms => Items;

    public IPlatform CreatePlatform(string aName) => createPlatform(aName, Guid.NewGuid());

    IPlatform IPlatformsContainer.CreatePlatform(string aName, Guid aGuid) => createPlatform(aName, aGuid);

    IPlatform IPlatformsContainer.GetPlatform(Guid aGuid) => GetItemByGuid(aGuid);

    private IPlatform createPlatform(string aName, Guid aGuid)
    {
        ValidateUniqueName(aName.ValidateNotNullOrWhitespace(), aP => aP.Name);

        return AddItem(new Platform(aName, aGuid), aGuid);
    }
}
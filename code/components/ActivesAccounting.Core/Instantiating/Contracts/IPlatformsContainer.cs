using System;
using System.Collections.Generic;

using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Contracts;

public interface IPlatformsContainer : IContainer<IPlatform>
{
    IReadOnlySet<IPlatform> Platforms { get; }

    IPlatform CreatePlatform(string aName);

    internal IPlatform CreatePlatform(string aName, Guid aGuid);
}
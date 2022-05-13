using System;
using System.Collections.Generic;

using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Contracts;

public interface IPlatformsContainer : IContainer
{
    IEnumerable<IPlatform> Platforms { get; }

    IPlatform CreatePlatform(string aName);

    internal IPlatform CreatePlatform(string aName, Guid aGuid);
    internal IPlatform GetPlatform(Guid aGuid);
}
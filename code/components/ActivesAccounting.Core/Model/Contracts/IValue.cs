using System;

namespace ActivesAccounting.Core.Model.Contracts;

public interface IValue : IEquatable<IValue>
{
    ICurrency Currency { get; }
    decimal Count { get; }
    IPlatform Platform { get; }
}
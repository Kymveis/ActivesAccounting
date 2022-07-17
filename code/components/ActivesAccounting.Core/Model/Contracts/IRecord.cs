using System;

using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.Core.Model.Contracts;

public interface IRecord : IUniqueItem
{
    DateTime DateTime { get; }
    RecordType RecordType { get; }
    IValue Source { get; }
    IValue Target { get; }
}
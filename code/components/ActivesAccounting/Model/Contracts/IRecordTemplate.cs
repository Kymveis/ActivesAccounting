using System;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.Model.Contracts;

public interface IRecordTemplate : ITemplate<IRecord>
{
    DateTime? DateTime { get; set; }
    RecordType? Type { get; set; }
    
    IPlatform? SourcePlatform { get; set; }
    ICurrency? SourceCurrency { get; set; }
    string? SourceValue { get; set; }
    
    IPlatform? TargetPlatform { get; set; }
    ICurrency? TargetCurrency { get; set; }
    string? TargetValue { get; set;}
}
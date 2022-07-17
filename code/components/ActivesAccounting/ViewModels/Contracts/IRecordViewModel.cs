namespace ActivesAccounting.ViewModels.Contracts;

internal interface IRecordViewModel
{
    string Date { get; }
    string Type { get; }
    string SourcePlatform { get; }
    string SourceValue { get; }
    string TargetPlatform { get; }
    string TargetValue { get; }
}
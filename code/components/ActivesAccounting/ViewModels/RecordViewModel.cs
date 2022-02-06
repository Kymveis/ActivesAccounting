using System;

using ActivesAccounting.Core.Model.Contracts;

using ValueType = ActivesAccounting.Core.Model.Enums.ValueType;

namespace ActivesAccounting.ViewModels
{
    internal sealed class RecordViewModel
    {
        private readonly IRecord _record;

        public RecordViewModel(IRecord aRecord)
        {
            _record = aRecord ?? throw new ArgumentNullException(nameof(aRecord));
            Date = aRecord.DateTime.ToString("dd MMM yyyy");
            Type = aRecord.RecordType.ToString();

            var (sourceCurrency, sourceCount) = getDescription(aRecord.Source);
            SourceCurrency = sourceCurrency;
            SourceCount = sourceCount;
            SourcePlatform = printPlatform(aRecord.Source.Platform);

            var (targetCurrency, targetCount) = getDescription(aRecord.Target);
            TargetCurrency = targetCurrency;
            TargetCount = targetCount;
            TargetPlatform = printPlatform(aRecord.Target.Platform);
        }

        public string Date { get; }
        public string Type { get; }

        public string SourcePlatform { get; }
        public string SourceCurrency { get; }
        public string SourceCount { get; }

        public string TargetPlatform { get; }
        public string TargetCurrency { get; }
        public string TargetCount { get; }

        (string Currency, string Count) getDescription(IValue aValue) => aValue.ValueType switch
        {
            ValueType.Simple when aValue is ISimpleValue simpleValue =>
                (printCurrency(simpleValue.Currency), simpleValue.Count.ToString()),
            _ => throw new ArgumentOutOfRangeException(nameof(aValue))
        };

        private static string printPlatform(IPlatform aPlatform) => aPlatform.Name;
        private static string printCurrency(ICurrency aCurrency) => $"{aCurrency.Name} ({aCurrency.Type})";
    }
}
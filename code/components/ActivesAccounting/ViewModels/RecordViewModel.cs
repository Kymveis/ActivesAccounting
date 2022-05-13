using System;

using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.ViewModels
{
    internal sealed class RecordViewModel
    {
        public RecordViewModel(IRecord aRecord)
        {
            Date = aRecord.DateTime.ToString("dd MMM yyyy");
            Type = aRecord.RecordType.ToString();

            SourceValue = printValue(aRecord.Source);
            SourcePlatform = printPlatform(aRecord.Source.Platform);

            TargetValue = printValue(aRecord.Target);
            TargetPlatform = printPlatform(aRecord.Target.Platform);
        }

        public string Date { get; }
        public string Type { get; }

        public string SourcePlatform { get; }
        public string SourceValue { get; }

        public string TargetPlatform { get; }
        public string TargetValue { get; }

        private static string printValue(IValue aValue) => $"{aValue.Count:N} {printCurrency(aValue.Currency)}";
        private static string printPlatform(IPlatform aPlatform) => aPlatform.Name;
        private static string printCurrency(ICurrency aCurrency) => $"{aCurrency.Name} ({aCurrency.Type})";
    }
}
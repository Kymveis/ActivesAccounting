using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Instantiating.Implementation
{
    internal sealed class ValueFactory : IValueFactory
    {
        private sealed record SimpleValue(ICurrency Currency, decimal Count) : ISimpleValue;

        private sealed record CombinedValue : ICombinedValue
        {
            public IReadOnlyCollection<ISimpleValue> SimpleValues { private get; init; }

            public int Count => SimpleValues.Count;
            public IEnumerator<ISimpleValue> GetEnumerator() => SimpleValues.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public ISimpleValue CreateSimpleValue(ICurrency aCurrency, decimal aCount) =>
            new SimpleValue(
                aCurrency.ValidateNotNull(nameof(aCurrency)),
                aCount.ValidateMoreThanZero(nameof(aCount)));

        public ICombinedValue CreateCombinedValue(IEnumerable<ISimpleValue> aSimpleValues) =>
            new CombinedValue {SimpleValues = aSimpleValues.ValidateNotNull(nameof(aSimpleValues)).ToImmutableArray()};
    }
}
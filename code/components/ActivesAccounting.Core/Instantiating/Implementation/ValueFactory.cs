using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Instantiating.Implementation
{
    internal sealed class ValueFactory : IValueFactory
    {
        private sealed record SimpleValue(ICurrency Currency, decimal Count, IPlatform Platform) : ISimpleValue;

        public ISimpleValue CreateSimpleValue(IPlatform aPlatform, ICurrency aCurrency, decimal aCount) =>
            new SimpleValue(
                aCurrency.ValidateNotNull(nameof(aCurrency)),
                aCount.ValidateMoreThanZero(nameof(aCount)),
                aPlatform.ValidateNotNull(nameof(aPlatform)));
    }
}
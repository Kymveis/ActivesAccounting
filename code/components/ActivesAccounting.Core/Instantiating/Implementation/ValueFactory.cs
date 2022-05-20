using System;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Instantiating.Implementation;

internal sealed class ValueFactory : IValueFactory
{
    private sealed record Value(ICurrency Currency, decimal Count, IPlatform Platform) : IValue
    {
        public bool Equals(IValue? aOther)
        {
            if (ReferenceEquals(null, aOther))
            {
                return false;
            }

            if (ReferenceEquals(this, aOther))
            {
                return true;
            }

            return Currency.Equals(aOther.Currency) 
                && Count == aOther.Count 
                && Platform.Equals(aOther.Platform);
        }

        public override int GetHashCode() => HashCode.Combine(Currency, Count, Platform);
    }

    public IValue CreateValue(IPlatform aPlatform, ICurrency aCurrency, decimal aCount) =>
        new Value(aCurrency, aCount.ValidateMoreThanZero(), aPlatform);
}
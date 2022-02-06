using System;
using System.Collections.Generic;
using System.Linq;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Instantiating.Implementation
{
    internal sealed class CurrenciesContainer : ContainerBase<ICurrency>, ICurrenciesContainer
    {
        private record Currency(string Name, CurrencyType Type, Guid Guid) : ICurrency;

        protected override string ItemName => "currency";

        public IEnumerable<ICurrency> Currencies => Items;

        public ICurrency CreateCurrency(string aName, CurrencyType aType) => createCurrency(aName, aType, Guid.NewGuid());

        ICurrency ICurrenciesContainer.CreateCurrency(string aName, CurrencyType aType, Guid aGuid) => createCurrency(aName, aType, aGuid);
        ICurrency ICurrenciesContainer.GetCurrency(Guid aCurrencyGuid) => GetItemByGuid(aCurrencyGuid);

        private ICurrency createCurrency(string aName, CurrencyType aType, Guid aGuid)
        {
            ValidateUniqueName(
                aName.ValidateNotNullOrWhitespace(nameof(aName)),
                aC => aC.Name);

            return AddItem(new Currency(aName, aType.ValidateEnum(nameof(aType), CurrencyType.Undefined), aGuid), aGuid);
        }
    }
}
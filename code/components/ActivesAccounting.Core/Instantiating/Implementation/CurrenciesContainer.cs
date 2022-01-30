using System;
using System.Collections.Generic;
using System.Linq;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Instantiating.Implementation
{
    internal sealed class CurrenciesContainer : ContainerBase<ICurrency>, ICurrenciesContainer
    {
        private record Currency(string Name, Guid Guid) : ICurrency;

        protected override string ItemName => "currency";

        public IEnumerable<ICurrency> Currencies => Items;

        public ICurrency CreateCurrency(string aName) => createCurrency(aName, Guid.NewGuid());

        ICurrency ICurrenciesContainer.CreateCurrency(string aName, Guid aGuid) => createCurrency(aName, aGuid);
        ICurrency ICurrenciesContainer.GetCurrency(Guid aCurrencyGuid) => GetItemByGuid(aCurrencyGuid);

        private ICurrency createCurrency(string aName, Guid aGuid)
        {
            aName.ValidateNotNullOrWhitespace(nameof(aName));
            if (Items.Any(i => i.Name.Equals(aName)))
            {
                throw Exceptions.AlreadyHasItem(ItemName, aName, "Name");
            }

            return AddItem(new Currency(aName, aGuid), aGuid);
        }
    }
}
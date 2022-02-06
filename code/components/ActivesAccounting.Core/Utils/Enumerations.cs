using System;
using System.Collections.Generic;

namespace ActivesAccounting.Core.Utils
{
    public static class Enumerations
    {
        public static void ForEach<T>(this IEnumerable<T> aItems, Action<T> aAction)
        {
            aAction.ValidateNotNull(nameof(aAction));
            foreach (var item in aItems.ValidateNotNull(nameof(aItems)))
            {
                aAction(item);
            }
        }
    }
}
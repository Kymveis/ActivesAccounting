using System;
using System.Collections.Generic;

namespace ActivesAccounting.Core.Utils;

public static class Enumerations
{
    public static void ForEach<T>(this IEnumerable<T> aItems, Action<T> aAction)
    {
        foreach (var item in aItems)
        {
            aAction(item);
        }
    }
}
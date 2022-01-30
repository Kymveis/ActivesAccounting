using System;

using Humanizer;

namespace ActivesAccounting.Core.Utils
{
    public static class Exceptions
    {
        public static Exception AlreadyHasItem<T>(string aItemName, T aKey, string? aKeyName = null) =>
            new InvalidOperationException($"Container already has {writeItem(aItemName, aKey, aKeyName)}.");

        public static Exception NotHasItem<T>(string aItemName, T aKey, string? aKeyName = null) =>
            new InvalidOperationException($"Container does not contain {writeItem(aItemName, aKey, aKeyName)}.");

        private static string writeItem<T>(string aItemName, T aKey, string? aKeyName) =>
            $"{aItemName.Humanize()} with {(aKeyName ?? aItemName).Camelize()} = {aKey}";
    }
}
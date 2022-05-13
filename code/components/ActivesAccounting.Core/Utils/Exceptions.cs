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

        public static Exception LessOrEqual<T>(T aValue, string aArgumentName, T aMinValue) where T : IComparable<T> =>
            new ArgumentOutOfRangeException(
                aArgumentName,
                aValue,
                $"Value cannot be less than or equal to {aMinValue}");
        
        private static string writeItem<T>(string aItemName, T aKey, string? aKeyName) =>
            $"{aItemName.Humanize()} with {(aKeyName ?? aItemName).Pascalize()} = {aKey}";
    }
}
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ActivesAccounting.Core.Utils
{
    public static class Validating
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ValidateEnum<T>(this T aEnum, string aArgumentName, T aUndefinedValue) where T : struct, Enum
        {
            if (!Enum.IsDefined(aEnum))
            {
                throw new InvalidEnumArgumentException(aArgumentName, (int) (object) aEnum, typeof(T));
            }

            if (aEnum.Equals(aUndefinedValue))
            {
                throw new ArgumentOutOfRangeException(aArgumentName);
            }

            return aEnum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal ValidateMoreThanZero(this decimal aValue, string aArgumentName)
        {
            const decimal ZERO_VALUE = 0;
            return aValue > ZERO_VALUE ? aValue : throw createLessOrEqualException(aValue, aArgumentName, ZERO_VALUE);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ValidateMinValue<T>(this T aValue, string aArgumentName, T aMinValue, bool aIncluding)
            where T : IComparable<T>
        {
            var comparisonResult = aValue.CompareTo(aMinValue);
            if (aIncluding)
            {
                if (comparisonResult == -1)
                {
                    throw new ArgumentOutOfRangeException(aArgumentName, aValue,
                        $"Value cannot be less than {aMinValue}.");
                }
            }
            else
            {
                if (comparisonResult <= -1)
                {
                    throw createLessOrEqualException(aValue, aArgumentName, aMinValue);
                }
            }

            return aValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ValidateNotNull<T>(this T aValue, string aArgumentName) where T : class =>
            aValue ?? throw new ArgumentNullException(aArgumentName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ValidateNotNullOrWhitespace(this string aString, string aArgumentName) =>
            string.IsNullOrWhiteSpace(aString)
                ? throw new ArgumentException("Value cannot be null or whitespace.", aArgumentName)
                : aString;

        private static ArgumentOutOfRangeException createLessOrEqualException<T>(T aValue, string aArgumentName,
            T aMinValue) where T : IComparable<T> =>
            new(aArgumentName, aValue, $"Value cannot be less than or equal to {aMinValue}");
    }
}
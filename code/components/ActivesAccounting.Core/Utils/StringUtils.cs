using System.Collections.Generic;

namespace ActivesAccounting.Core.Utils;

public static class StringUtils
{
    public static string JoinStrings(this IEnumerable<string> aStrings, string aSeparator = "\n") =>
        string.Join(aSeparator, aStrings);
}
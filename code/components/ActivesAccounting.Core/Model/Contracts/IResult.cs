using System.Collections.Generic;

namespace ActivesAccounting.Core.Model.Contracts;

public interface IResult<out T>
{
    bool Valid { get; }
    T Value { get; }
    IReadOnlyList<string> Errors { get; }
}
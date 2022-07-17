using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Model.Implementation;

public sealed class Result<T> : IResult<T>
{
    private readonly T? _result;

    public Result(T aValue)
    {
        _result = aValue;
        Errors = ImmutableList<string>.Empty;
    }

    public Result(string aError) => Errors = new List<string> {aError}.AsReadOnly();

    public Result(IEnumerable<string> aErrors) => Errors = aErrors.ToImmutableList();

    public bool Valid => _result is not null;

    public T Value => _result ?? throw new InvalidOperationException(
        $"The value wasn't created due to the following error(s): {Errors.JoinStrings(";")}");

    public IReadOnlyList<string> Errors { get; }
}
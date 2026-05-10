using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noname.Application.Common.Models;

public class Result
{
    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    public bool Succeeded { get; init; }

    public string[] Errors { get; init; }

    public static Result Success() => new Result(true, Array.Empty<string>());
    public static Result Failure(IEnumerable<string> errors) => new Result(false, errors);
    public static Result Failure(string error) => new(false, new[] { error });

}

public class Result<T> : Result
{
    internal Result(T? data, bool succeeded, IEnumerable<string> errors)
        : base(succeeded, errors)
    {
        Data = data;
    }

    public T? Data { get; init; }

    public static Result<T> Success(T data) => new(data, true, Array.Empty<string>());
    public new static Result<T> Failure(IEnumerable<string> errors) => new(default, false, errors);
    public new static Result<T> Failure(string error) => new(default, false, new[] { error });
}
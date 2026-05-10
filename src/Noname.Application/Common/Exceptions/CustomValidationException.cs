using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noname.Application.Common.Exceptions;

public class CustomValidationException : Exception
{
    public CustomValidationException(IEnumerable<string> errors)
        : base("One or more validation failures have occurred.")
    {
        Errors = errors.ToArray();
    }

    public string[] Errors { get; }
}

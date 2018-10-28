using System;
using System.Collections.Generic;

namespace TypedRestClient.Filters.Validation
{
    public interface IValidationFilterEventArgs
    {
        IReadOnlyDictionary<string, RequestParameter> Parameters { get; }
        Type ReturnType { get; }
    }
}

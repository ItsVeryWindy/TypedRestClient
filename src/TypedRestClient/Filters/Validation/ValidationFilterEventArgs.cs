using System;
using System.Collections.Generic;

namespace TypedRestClient.Filters.Validation
{
    internal class ValidationFilterEventArgs : IValidationFilterEventArgs
    {
        public IReadOnlyDictionary<string, RequestParameter> Parameters { get; }
        public Type ReturnType { get; }

        public ValidationFilterEventArgs(IReadOnlyDictionary<string, RequestParameter> parameters, Type returnType)
        {
            Parameters = parameters;
            ReturnType = returnType;
        }
    }
}

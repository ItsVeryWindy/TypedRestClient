using System;
using System.Collections.Generic;

namespace ClassLibrary1.Filters
{
    public interface IValidationFilterEventArgs
    {
        Type ConstructorParameters { get; }
        IReadOnlyDictionary<string, RequestParameter> Parameters { get; }
        Type ReturnType { get; }
    }

    public class ValidationFilterEventArgs : IValidationFilterEventArgs
    {
        public Type ConstructorParameters { get; }
        public IReadOnlyDictionary<string, RequestParameter> Parameters { get; }
        public Type ReturnType { get; }

        public ValidationFilterEventArgs(Type constructorParameters, IReadOnlyDictionary<string, RequestParameter> parameters, Type returnType)
        {
            ConstructorParameters = constructorParameters;
            Parameters = parameters;
            ReturnType = returnType;
        }
    }
}

using System;
using System.Collections.Generic;

namespace TypedRestClient
{
    public class TypedRestClientConfiguration
    {
        private readonly Dictionary<Type, object> _configuration = new Dictionary<Type, object>();

        public T Get<T>() where T : new()
        {
            if (_configuration.TryGetValue(typeof(T), out var value))
                return (T)value;

            value = new T();

            _configuration[typeof(T)] = value;

            return (T)value;
        }
    }
}

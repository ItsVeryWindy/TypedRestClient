using System;
using System.Collections.Generic;
using System.Linq;

namespace TypedRestClient.Filters.Validation
{
    public class RequestParameter
    {
        public RequestParameter(Type type, IEnumerable<Attribute> attributes)
        {
            Type = type;
            Attributes = attributes.ToArray();
        }

        public Type Type { get; }
        public Attribute[] Attributes { get; }
    }
}

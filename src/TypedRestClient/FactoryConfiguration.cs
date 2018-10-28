using System;
using System.Collections.Generic;
using TypedRestClient.Filters.Requests;
using TypedRestClient.Filters.Responses;

namespace TypedRestClient
{
    public class FactoryConfiguration
    {
        internal List<object> Filters { get; } = new List<object>();

        public FactoryConfiguration AddFilter(object filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            if(filter is IRequestFilter)
            {
                Filters.Add(filter);
                return this;
            }

            if (filter is IRequestFilterAsync)
            {
                Filters.Add(filter);
                return this;
            }

            if (filter is IResponseFilter)
            {
                Filters.Add(filter);
                return this;
            }

            if (filter is IResponseFilterAsync)
            {
                Filters.Add(filter);
                return this;
            }

            throw new ArgumentException(nameof(filter), "Invalid filter");
        }
    }
}

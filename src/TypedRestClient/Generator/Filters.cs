using System.Collections.Generic;
using TypedRestClient.Filters.Exceptions;
using TypedRestClient.Filters.Requests;
using TypedRestClient.Filters.Responses;

namespace TypedRestClient.Generator
{
    public class Filters
    {
        public List<IRequestFilterAsync> RequestFilters { get; } = new List<IRequestFilterAsync>();
        public List<IResponseFilterAsync> ResponseFilters { get; } = new List<IResponseFilterAsync>();
        public List<IExceptionFilterAsync> ExceptionFilters { get; } = new List<IExceptionFilterAsync>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypedRestClient.Filters.Requests;
using TypedRestClient.Filters.Validation;

namespace TypedRestClient.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class HttpRouteAttribute : Attribute, IRequestFilter, IValidationFilter
    {
        private readonly RouteComponent[] _routeParameters;
        private readonly int _strLength;

        private enum ParameterType
        {
            Constant,
            Parameter
        }

        private class RouteComponent
        {
            public string String { get; set; }
            public ParameterType Type { get; set; }
        }

        public HttpRouteAttribute(string route)
        {
            _routeParameters = CreateComponents(route);

            _strLength = CalculateStringBuilderLength();
        }

        public bool Validate(IValidationFilterEventArgs eventArgs)
        {
            return _routeParameters.All(component => component.Type != ParameterType.Parameter || eventArgs.Parameters.ContainsKey(component.String));
        }

        private static RouteComponent[] CreateComponents(string route)
        {
            var components = new List<RouteComponent>();

            for (var i = 0; i < route.Length;)
            {
                if (IsParameterComponent(route, i))
                {
                    components.Add(ParseParameterComponent(route, ref i));
                }
                else
                {
                    var component = ParseStringComponent(route, ref i);

                    if (component != null)
                    {
                        components.Add(component);
                    }
                }
            }

            return components.ToArray();
        }

        private static bool IsParameterComponent(string route, int index)
        {
            if (route[index] != '{')
                return false;

            var nextIndex = index + 1;

            if (nextIndex >= route.Length)
                return true;

            return route[nextIndex] != '{';
        }

        private static RouteComponent ParseParameterComponent(string route, ref int index)
        {
            for (var start = ++index; index < route.Length; index++)
            {
                if (route[index] == '}')
                {
                    index++;

                    return new RouteComponent
                    {
                        String = route.Substring(start, index - 1 - start),
                        Type = ParameterType.Parameter
                    };
                }
            }

            throw new InvalidOperationException();
        }

        private static RouteComponent ParseStringComponent(string route, ref int index)
        {
            var start = index;

            for (; index < route.Length; index++)
            {
                if (IsParameterComponent(route, index))
                    break;
            }

            if (index - start == 0)
                return null;

            return new RouteComponent
            {
                String = route.Substring(start, index - start),
                Type = ParameterType.Constant
            };
        }

        private int CalculateStringBuilderLength()
        {
            return _routeParameters.Sum(c => c.Type == ParameterType.Parameter ? 16 : c.String.Length);
        }

        private Uri BuildRequestUri(IReadOnlyDictionary<string, object> parameters)
        {
            var builder = new StringBuilder(_strLength);

            foreach (var component in _routeParameters)
            {
                if (component.Type == ParameterType.Parameter)
                {
                    builder.Append(parameters[component.String]);
                }
                else
                {
                    builder.Append(component.String);
                }
            }

            return new Uri(builder.ToString(), UriKind.Relative);
        }

        public virtual void OnRequest(IRequestEventArgs eventArgs)
        {
            eventArgs.Request.RequestUri = BuildRequestUri(eventArgs.Parameters);
        }
    }
}

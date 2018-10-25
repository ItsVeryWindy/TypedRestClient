using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    interface IHttpClientBuilder
    {
        IHttpClientBuilderOptions<TParams, TReturn> Create<TParams, TReturn>();
    }

    interface IHttpClientBuilderOptions<TReturn>
    {
        IHttpClientBuilderOptions<TReturn> OnRequest(Action<HttpRequestMessage> action);
        IHttpClientBuilderOptions<TReturn> OnRequest(Func<HttpRequestMessage, Task> action);
        IHttpClientBuilderOptions<TReturn> OnResponse(Predicate<HttpResponseMessage> condition, Func<HttpResponseMessage, TReturn> action);
        IHttpClientBuilderOptions<TReturn> OnResponse(Predicate<HttpResponseMessage> condition, Func<Task<TReturn>> action);
        IHttpClientBuilderOptions<TReturn> OnException(Func<Exception, TReturn> action);
        IHttpClientBuilderOptions<TReturn> OnException(Func<Exception, Task<TReturn>> action);
        Func<TReturn> Build();
    }

    public interface IHttpClientBuilderOptions<TParams, TReturn>
    {
        IHttpClientBuilderOptions<TParams, TReturn> OnRequest(Action<HttpRequestMessage, TParams> action);
        IHttpClientBuilderOptions<TParams, TReturn> OnRequest(Func<HttpRequestMessage, TParams, Task> action);
        IHttpClientBuilderOptions<TParams, TReturn> OnResponse(Predicate<HttpResponseMessage> condition, Func<HttpResponseMessage, TReturn> action);
        IHttpClientBuilderOptions<TParams, TReturn> OnResponse(Predicate<HttpResponseMessage> condition, Func<HttpResponseMessage, Task<TReturn>> action);
        IHttpClientBuilderOptions<TParams, TReturn> OnException(Func<Exception, TReturn> action);
        IHttpClientBuilderOptions<TParams, TReturn> OnException(Func<Exception, Task<TReturn>> action);
        Func<TParams, TReturn> Build();
    }

    public static class HttpClientBuilderOptionsExtensions
    {
        public static IHttpClientBuilderOptions<TParams, TReturn> SetRoute<TParams, TReturn>(
            this IHttpClientBuilderOptions<TParams, TReturn> options, Func<TParams, FormattableString> func)
        {
            return options.OnRequest((request, parameters) =>
            {
                var format = func(parameters);
                var args = format.GetArguments().Select(a => a == null ? null : (object)Uri.EscapeUriString(a.ToString())).ToArray();

                request.RequestUri = new Uri(string.Format(format.Format, args), UriKind.Relative);
            });
        }

        public static IHttpClientBuilderOptions<TParams, TReturn> SetMethod<TParams, TReturn>(
            this IHttpClientBuilderOptions<TParams, TReturn> options, HttpMethod method)
        {
            return options.OnRequest((request, parameters) => request.Method = method);
        }

        public static IHttpClientBuilderOptions<TParams, TReturn> Get<TParams, TReturn>(
            this IHttpClientBuilderOptions<TParams, TReturn> options, Func<TParams, FormattableString> func)
        {
            return options
                .SetRoute(func)
                .SetMethod(HttpMethod.Get);
        }

        public static IHttpClientBuilderOptions<TParams, TReturn> JsonResponse<TParams, TReturn>(
            this IHttpClientBuilderOptions<TParams, TReturn> options)
        {
            return options.OnResponse(response => response.IsSuccessStatusCode, response =>
            {
                return (TReturn)(object)response.Content.ReadAsStringAsync();
            });
        }
    }

    public class HttpClientBuilderOptions<TParams, TReturn> : IHttpClientBuilderOptions<TParams, TReturn>
    {
        private IList<Func<HttpRequestMessage, TParams, Task>> RequestBuilders { get; }
        private IList<Func<HttpResponseMessage, Task<TReturn>>> ResponseBuilders { get; }
        private IList<Func<Exception, Task<TReturn>>> ExceptionBuilders { get; }

        public HttpClientBuilderOptions()
        {
            RequestBuilders = new List<Func<HttpRequestMessage, TParams, Task>>();
            ResponseBuilders = new List<Func<HttpResponseMessage, Task<TReturn>>>();
            ExceptionBuilders = new List<Func<Exception, Task<TReturn>>>();
        }


        public IHttpClientBuilderOptions<TParams, TReturn> OnRequest(Action<HttpRequestMessage, TParams> action)
        {
            RequestBuilders.Add((request, parameters) =>
            {
                action(request, parameters);
                return Task.CompletedTask;
            });

            return this;
        }

        public IHttpClientBuilderOptions<TParams, TReturn> OnRequest(Func<HttpRequestMessage, TParams, Task> action)
        {
            throw new NotImplementedException();
        }

        public IHttpClientBuilderOptions<TParams, TReturn> OnResponse(Predicate<HttpResponseMessage> condition, Func<HttpResponseMessage, TReturn> action)
        {
            ResponseBuilders.Add(response =>
            {
                var result = action(response);
                return Task.FromResult(result);
            });

            return this;
        }

        public IHttpClientBuilderOptions<TParams, TReturn> OnResponse(Predicate<HttpResponseMessage> condition, Func<HttpResponseMessage, Task<TReturn>> action)
        {
            throw new NotImplementedException();
        }

        public IHttpClientBuilderOptions<TParams, TReturn> OnException(Func<Exception, TReturn> action)
        {
            throw new NotImplementedException();
        }

        public IHttpClientBuilderOptions<TParams, TReturn> OnException(Func<Exception, Task<TReturn>> action)
        {
            throw new NotImplementedException();
        }

        public Func<TParams, TReturn> Build()
        {
            return Build;
        }

        private TReturn Build(TParams @params)
        {
            var request = new HttpRequestMessage();

            foreach (var requestBuilder in RequestBuilders)
            {
                requestBuilder(request, @params);
            }

            return default(TReturn);
        }
    }
}

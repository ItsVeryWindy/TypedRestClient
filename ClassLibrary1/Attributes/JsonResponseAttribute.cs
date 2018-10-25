using System;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary1.Filters;
using Newtonsoft.Json;

namespace ClassLibrary1.Attributes
{
    /// <summary>
    /// Deserializes the body of the response from JSON into the return type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class JsonResponseAttribute : Attribute, IResponseFilterAsync
    {
        public async Task OnResponseAsync<TReturn>(IResponseEventArgs<TReturn> eventArgs, CancellationToken cancellationToken)
        {
            eventArgs.ReturnValue = JsonConvert.DeserializeObject<TReturn>(await eventArgs.Response.Content.ReadAsStringAsync());
        }
    }
}

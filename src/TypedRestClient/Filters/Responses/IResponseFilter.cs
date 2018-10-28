namespace TypedRestClient.Filters.Responses
{
    public interface IResponseFilter
    {
        void OnResponse<TReturn>(IResponseEventArgs<TReturn> eventArgs);
    }
}

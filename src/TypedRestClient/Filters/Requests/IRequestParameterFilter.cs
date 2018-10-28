namespace TypedRestClient.Filters.Requests
{
    public interface IRequestParameterFilter
    {
        void OnRequest(IRequestParameterEventArgs eventArgs);
    }
}

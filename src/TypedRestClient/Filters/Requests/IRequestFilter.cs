namespace TypedRestClient.Filters.Requests
{
    public interface IRequestFilter
    {
        void OnRequest(IRequestEventArgs eventArgs);
    }
}

namespace TypedRestClient.Filters.Exceptions
{
    public interface IExceptionFilter
    {
        void OnException(IExceptionEventArgs eventArgs);
    }
}

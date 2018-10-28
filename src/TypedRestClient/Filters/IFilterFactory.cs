namespace TypedRestClient.Filters
{
    public interface IFilterFactory
    {
        object CreateFilter(TypedRestClientConfiguration configuration);
    }
}

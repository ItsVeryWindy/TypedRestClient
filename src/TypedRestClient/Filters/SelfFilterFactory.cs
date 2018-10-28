namespace TypedRestClient.Filters
{
    internal class SelfFilterFactory : IFilterFactory
    {
        private readonly object _filter;

        public SelfFilterFactory(object filter)
        {
            _filter = filter;
        }

        public object CreateFilter(TypedRestClientConfiguration configuration)
        {
            return _filter;
        }
    }
}

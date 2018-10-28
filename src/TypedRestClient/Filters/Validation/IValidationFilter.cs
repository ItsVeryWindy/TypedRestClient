namespace TypedRestClient.Filters.Validation
{
    public interface IValidationFilter
    {
        bool Validate(IValidationFilterEventArgs eventArgs);
    }
}

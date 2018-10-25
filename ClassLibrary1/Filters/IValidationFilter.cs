using ClassLibrary1.Filters;

namespace ClassLibrary1
{
    public interface IValidationFilter
    {
        bool Validate(IValidationFilterEventArgs eventArgs);
    }
}

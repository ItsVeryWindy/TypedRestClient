using System;

namespace ClassLibrary1.Attributes
{
    public interface IDependencyInjectionConstructorParameters
    {
        IServiceProvider ServiceProvider { get; set; }
    }
}

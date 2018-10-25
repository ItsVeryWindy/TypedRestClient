using System.Net.Http;

namespace ClassLibrary1
{
    public interface IConstructorFilter<in TConstructorParameters>
    {
        void Initialize(HttpClient client, TConstructorParameters constructor);
    }
}

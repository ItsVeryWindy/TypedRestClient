namespace ClassLibrary1
{
    public interface IHttpClientProxyFactoy<TInterface, TAdditionalConstructorParameters>
    {
        TInterface Create(TAdditionalConstructorParameters parameters);
    }
}
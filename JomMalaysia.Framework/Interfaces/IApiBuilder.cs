namespace JomMalaysia.Framework.Interfaces
{
    public interface IApiBuilder
    {
        string WebApiUrl { get; }
        string GetApi(string path, params string[] parameters);
    }
}


namespace PharmaMoov.Admin.DataLayer
{
    public interface IMainHttpClient
    {
        string PostHttpClientRequest(string _rEndPoint, object _content);
        string GetHttpClientRequest(string requestEndPoint);
        string PostFileHttpCientRequest(string _rEndPoint, System.Net.Http.HttpContent _content);
        string GetAnnonHttpClientRequest(string requestEndPoint);
    }
}
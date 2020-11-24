using System.Net.Http;

namespace SearchService
{

    public interface ISearchHttpClient
    { 
    }

    /// <summary>
    /// A Https-client used to connect to search engine and download the first result page.
    /// The userAgent string should be like a modern browser or else the webserver won't return the hit count.
    /// </summary>
    public class SearchHttpClient : HttpClient, ISearchHttpClient
    {
        const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36";

        public SearchHttpClient()
        {
            DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
        }

    }
}

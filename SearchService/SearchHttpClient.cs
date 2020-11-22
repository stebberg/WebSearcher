using System.Net.Http;
using System.Threading.Tasks;

namespace SearchService
{
    /// <summary>
    /// A Https-client used to connect to search engine and download the first result page.
    /// The userAgent string should be like a modern browser or else the webserver won't return the hit count.
    /// </summary>
    public class SearchHttpClient
    {
        const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36";
        public async Task<string> DownloadWebPage(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
                return await client.GetStringAsync(url);
            }
        }
    }
}

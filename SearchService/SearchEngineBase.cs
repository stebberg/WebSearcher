using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchService
{

    /// <summary>
    /// Abstract class to use when creating search engines
    /// It's possible to override the GetNumberFromHitText-function if the search enginges result text needs special cleanup when retracting the hit-count number.
    /// </summary>
    public abstract class SearchEngineBase
    {
        public abstract string ProviderName { get; }
        public abstract string GetUrl(string query);
        public abstract string HitCountRegEx { get; }

        public long? Result { get; set; }

        public async Task Search(string query)
        {
            Console.WriteLine($"{DateTime.Now} {ProviderName} starting download");
            string url = GetUrl(query);
            SearchHttpClient client = new SearchHttpClient();
            string pageData = await client.DownloadWebPage(url);
            Console.WriteLine($"{DateTime.Now} {ProviderName} done download");

            var match = new Regex(HitCountRegEx).Match(pageData);
            if (!match.Success)
                return;
            Console.WriteLine($"{DateTime.Now} {ProviderName} done function");
            Result = GetNumberFromHitText(match.Value);
        }
        public virtual long GetNumberFromHitText(string text)
        {
            StringBuilder sb = new StringBuilder("0");
            foreach (var c in text)
                if (IsDigit(c))
                    sb.Append(c);
            return long.Parse(sb.ToString());
        }

        bool IsDigit(char c) =>
            c >= '0' && c <= '9'; // we do not want to allow: , . -

        protected string UrnEncode(string url) =>
            FormattingHelper.UrlEncode(url);
    }





}

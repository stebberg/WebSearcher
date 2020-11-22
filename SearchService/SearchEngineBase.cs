using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchService
{

    /// <summary>
    /// Abstract class to use when creating search engines
    /// It's possible to override the "GetNumberFromHitText"-function if the search enginge's 
    /// resulttext needs special cleanup when retracting the hit-count-number.
    /// </summary>
    public abstract class SearchEngineBase
    {
        public abstract string ProviderName { get; }
        public abstract string GetUrl(string query);
        public abstract string HitCountRegEx { get; }

        public long? Result { get; set; }

        public async Task Search(string query)
        {
            var url = GetUrl(query);
            var client = new SearchHttpClient();
            var pageData = await client.DownloadWebPage(url);

            var match = new Regex(HitCountRegEx).Match(pageData);
            if (match.Success)
                Result = GetNumberFromHitText(match.Value);
        }

        public virtual long GetNumberFromHitText(string text)
        {
            var sb = new StringBuilder("0");
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

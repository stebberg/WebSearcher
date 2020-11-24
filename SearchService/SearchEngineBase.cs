using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchService
{

    /// <summary>
    /// Abstract class to use when adding search engines.
    /// It's possible to override the "GetNumberFromHitText"-function if the search engine's 
    /// result needs special cleanup when retracting the hit-count.
    /// </summary>
    public abstract class SearchEngineBase
    {
        public abstract string ProviderName { get; }
        public abstract string GetUrl(string query);
        public abstract string HitCountRegEx { get; }

        public long? Result { get; set; }

        public async Task Search(string queryWord)
        {
            var url = GetUrl(queryWord);

            using (var client = new SearchHttpClient())
            {
                var pageData = await client.GetStringAsync(url);

                var match = new Regex(HitCountRegEx).Match(pageData);
                if (match.Success)
                    Result = GetNumberFromHitText(match.Value);
            }
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
            c >= '0' && c <= '9'; // we do not want to include: , . -

        protected string UrnEncode(string url) =>
            FormattingHelper.UrlEncode(url);
    }





}

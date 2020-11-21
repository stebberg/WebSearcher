using System.Web;

namespace SearchService
{
    public class FormattingHelper
    {
        /// <summary>
        /// Format a number as text eg:
        ///     100 > 100
        ///     1500 > 1,5k
        ///     1230000 > 1,2M
        /// </summary>
        /// <param name="number"></param>
        /// <returns>textual representation of numbers</returns>
        public static string NumAsString(long number)
        {
            if (number < 0) throw new System.Exception("Can't handle negative numbers");
            if (number > 1_000_000)
                return $"{number / 1_000_000M:### ##0.0}M".Trim();
            if (number > 1_000)
                return $"{number / 1_000M:### ##0.0}k".Trim();
            return $"{number:### ##0.0}".Trim();
        }
        
        /// <summary>
        /// URL encode a string to it can be used in an URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns>encoding a string to URL</returns>
        public static string UrlEncode(string url) =>
            HttpUtility.UrlEncode(url);

    }
}

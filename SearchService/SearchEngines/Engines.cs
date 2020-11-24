using System;
using System.Collections.Generic;
using System.Text;

namespace SearchService.SearchEngines
{

    class GoogleSearch : SearchEngineBase
    {
        public override string ProviderName { get; } = "Google";
        public override string HitCountRegEx { get; } = @"<div id=""result-stats"">.+?<nobr>"; // <div id="result-stats">Ungefär 24 800 000 resultat<nobr> (0,52 sekunder)&nbsp;</nobr></div>
        public override string GetUrl(string query) => $"https://www.google.com/search?q={UrnEncode(query)}";
    }

    class BingSearch : SearchEngineBase
    {
        public override string ProviderName { get; } = "Bing";
        public override string HitCountRegEx { get; } = @"<span class=""sb_count"">.+?</span>"; // <span class="sb_count">298&#160;000 resultat</span>
        public override string GetUrl(string query) => $"https://www.bing.com/search?q={UrnEncode(query)}";
        public override long GetNumberFromHitText(string text) =>
            base.GetNumberFromHitText(text.Replace("&#160;", string.Empty)); // for Bing we need to ignore that separating no-break-space: &#160;
    }

    class YahooSearch : SearchEngineBase
    {
        public override string ProviderName { get; } = "Yahoo";
        public override string HitCountRegEx { get; } = @"<span>.+? result.*?</span>"; // <span>297,000 resultat</span>
        public override string GetUrl(string query) => $"https://se.search.yahoo.com/search?p={UrnEncode(query)}";
    }

}

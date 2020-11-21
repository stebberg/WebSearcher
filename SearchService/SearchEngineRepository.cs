using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchService
{
    public interface ISearchEngineRepository
    {
        SearchEngineBase CreateNewSearchEngineByName(string name);
        IEnumerable<string> GetAllSearchEnginesByNames();
    }

    /// <summary>
    /// Dummy repository with hardcoded search engines.
    /// When loading the repository we get all search engines by reflection
    /// </summary>
    public class MockSearchEngineRepository : ISearchEngineRepository
    {
        List<SearchEngineBase> searchEngines = new List<SearchEngineBase>();

        public MockSearchEngineRepository()
        {
            foreach (var searchEngineType in FindSubClassesOf<SearchEngineBase>())
                searchEngines.Add((SearchEngineBase)Activator.CreateInstance(searchEngineType));
        }

        public SearchEngineBase CreateNewSearchEngineByName(string name)
        {
            var searchEngine = searchEngines.FirstOrDefault(x => x.ProviderName.Equals(name, StringComparison.OrdinalIgnoreCase));
            return (SearchEngineBase)Activator.CreateInstance(searchEngine.GetType());
        }

        public IEnumerable<Type> FindSubClassesOf<TBaseType>()
        {
            var baseType = typeof(TBaseType);
            var assembly = baseType.Assembly;
            return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
        }

        public IEnumerable<string> GetAllSearchEnginesByNames() =>
            searchEngines.Select(x => x.ProviderName);

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


}

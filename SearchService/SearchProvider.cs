using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchService
{
    public interface ISearchProvider
    {
        Task<IEnumerable<Result>> DoSearch(IEnumerable<string> searchEngines, IEnumerable<string> queryWords);
    }

    /// <summary>
    /// The search provider is queueing up all the search requests per search engine and search term.
    /// All requests are then made simultaneously.
    /// </summary>
    public class SearchProvider : ISearchProvider
    {
        ISearchEngineRepository _searchEngineRepository;
        public SearchProvider(ISearchEngineRepository searchEngineRepository)
        {
            _searchEngineRepository = searchEngineRepository;
        }

        public async Task<IEnumerable<Result>> DoSearch(IEnumerable<string> searchEngines, IEnumerable<string> queryTerms)
        {
            List<(SearchEngineBase, string)> tasks = new List<(SearchEngineBase, string)>();
            foreach (var strSearchEngine in searchEngines)
            {
                foreach (var queryWord in queryTerms)
                {
                    if (string.IsNullOrWhiteSpace(queryWord)) continue;

                    var searchEngine = _searchEngineRepository.CreateNewSearchEngineByName(strSearchEngine);
                    if (searchEngine == null) continue;

                    tasks.Add((searchEngine, queryWord));
                }
            }

            if (tasks.Count > 0)
                await Task.WhenAll(tasks.Select(x => x.Item1.Search(x.Item2)));

            return tasks.Select(x => new Result 
            { 
                SearchEngine = x.Item1, 
                QueryWord = x.Item2, 
                HitCount = x.Item1.Result 
            } );
        }

    }
}

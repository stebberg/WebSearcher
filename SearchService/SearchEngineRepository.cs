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
            if (searchEngine == null)
                return null;
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

    }


}

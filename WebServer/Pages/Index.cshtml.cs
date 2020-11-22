using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SearchService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebServer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ISearchEngineRepository _searchEngineRepository;
        private readonly ISearchProvider _searchProvider;

        public IndexModel(ISearchEngineRepository searchEngineRepository, ISearchProvider searchProvider)
        {
            _searchEngineRepository = searchEngineRepository;
            _searchProvider = searchProvider;
        }

        [BindProperty(SupportsGet = true)]
        public string Query { get; set; }

        [BindProperty(SupportsGet = true)]
        public string[] SearchEngines { get; set; }

        public IEnumerable<string> AllSearchEngines => 
            _searchEngineRepository.GetAllSearchEnginesByNames();
        
        public Task<IEnumerable<Result>> Result =>
            GetResultFromSearch();

        async Task<IEnumerable<Result>> GetResultFromSearch() =>
            await _searchProvider.DoSearch(SearchEngines, SplitQueryIntoWords(Query));

        string[] SplitQueryIntoWords(string query) =>
            (query ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries);

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/Index", new { Query, SearchEngines });
        }

    }
}

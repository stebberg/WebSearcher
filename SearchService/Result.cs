namespace SearchService
{
    /// <summary>
    /// Class to hold the result from 1 search (=1 search engine + 1 search word)
    /// </summary>
    public class Result
    {
        public SearchEngineBase SearchEngine;
        public string QueryWord;
        public long? HitCount;
    }
}

using AutoMapper;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebParser.Api.Storage;

namespace WebParser.Api.Scan
{
    /// <summary>
    /// Data extractor for extracting data from web page.
    /// Its an initial working copy. I can not claim this as the best solution.
    /// There are many ways we can improve this. We can discuss if you like to talk more on this.
    /// </summary>
    public class DataExtractor : IDataExtractor
    {
        private readonly IStorageManager _storageManager;
        private readonly IMapper _mapper;
        private readonly ILogger<DataExtractor> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="storageManager">Storage interface</param>
        /// <param name="mapper">Automapper</param>
        ///  <param name="logger">Logger</param>
        public DataExtractor(IStorageManager storageManager, IMapper mapper, ILogger<DataExtractor> logger)
        {
            _storageManager = storageManager;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Private method used to extract text content and links in the page
        /// </summary>
        /// <param name="url">Url to extract</param>
        /// <param name="extractLinks">Flag whether to extract links</param>
        /// <returns>Text and links</returns>
        async Task<Tuple<string, IEnumerable<string>>> GetText(string url, bool extractLinks)
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);
            var sb = new StringBuilder();
            IEnumerable<HtmlNode> nodes = doc.DocumentNode.Descendants().Where(n =>
               n.NodeType == HtmlNodeType.Text &&
               n.ParentNode.Name != "script" &&
               n.ParentNode.Name != "style");
            foreach (HtmlNode node in nodes)
            {
                sb.AppendLine(node.InnerText);
            }
            string text = sb.ToString();
            if (extractLinks)
            {
                var linkedPages = doc.DocumentNode.Descendants("a")
                            .Select(a => a.GetAttributeValue("href", null))
                            .Where(u => !string.IsNullOrEmpty(u));
                return Tuple.Create(text, linkedPages);
            }
            else
            {
                return Tuple.Create(text, (new List<string>()).AsEnumerable());
            }
        }

        /// <summary>
        /// Get current word and word pair count
        /// </summary>
        /// <param name="scanJobEntity">Scan job</param>
        /// <param name="words">List of words</param>
        /// <param name="wordPairs">List of word pairs</param>
        void UpdateCount(ScanJobEntity scanJobEntity, Dictionary<string, int> words, Dictionary<string, int> wordPairs)
        {
            scanJobEntity.TopWords = (from w in words
                                      orderby w.Value descending
                                      select new TopWordEntity() { Word = w.Key, Count = w.Value }).Take(10).ToList();
            scanJobEntity.TopWordPairs = (from w in wordPairs
                                          orderby w.Value descending
                                          select new TopPairEntity()
                                          {
                                              Word = w.Key.Split(" ")[0],
                                              Second = w.Key.Split(" ")[1],
                                              Count = w.Value
                                          }).Take(10).ToList();
        }

        /// <summary>
        /// Processes inner text of the page and generates word and word pair counts
        /// </summary>
        /// <param name="innerText"></param>
        /// <param name="words"></param>
        /// <param name="wordPairs"></param>
        /// <returns></returns>
        async Task GetWords(string innerText, Dictionary<string, int> words, Dictionary<string, int> wordPairs)
        {
            if (!string.IsNullOrWhiteSpace(innerText))
            {
                var allWords = innerText.Split(" ").Where(c => !string.IsNullOrWhiteSpace(c)).Select(c => c.Trim()).ToArray();
                for (int i = 0; i < allWords.Count() - 1; i++)
                {
                    if (words.ContainsKey(allWords[i]))
                    {
                        words[allWords[i]]++;
                    }
                    else
                    {
                        words.Add(allWords[i], 1);
                    }

                    string pair = $"{allWords[i]} {allWords[i + 1]}";
                    if (wordPairs.ContainsKey(pair))
                    {
                        wordPairs[pair]++;
                    }
                    else
                    {
                        wordPairs.Add(pair, 1);
                    }
                }
            }
            await Task.Yield(); //Faking await inside async method
        }

        /// <summary>
        /// Extract data from webpage
        /// </summary>
        /// <param name="scanJob">Scan job details</param>
        /// <returns>Task</returns>
        public async Task Extract(ScanJob scanJob)
        {
            Uri uri = new Uri(scanJob.Url);
            var words = new Dictionary<string, int>();
            var wordPairs = new Dictionary<string, int>();
            var urls = new List<ExtractionStatus>() { new ExtractionStatus() { Url = scanJob.Url.ToLower().Trim(), Depth = 1, Picked = false } };
            scanJob.JobStatus = Common.ScanJobStatus.Inprogress;
            while (urls.Any(u => u.Depth <= 4 && !u.Picked))
            {
                var url = urls.FirstOrDefault(c => !c.Picked);

                if (url != null)
                {
                    try
                    {
                        var text = await GetText(url.Url, url.Depth <= 4);

                        await GetWords(text.Item1, words, wordPairs);
                        url.Picked = true;
                        if (text.Item2.Any())
                        {
                            
                            foreach (var u in text.Item2)
                            {
                                var tempUrl = u.ToLower().Trim();
                                if ((tempUrl.Contains("https") || tempUrl.Contains("http"))
                                    && tempUrl.Contains(uri.Host.ToLower())
                                    && !urls.Any(c => c.Url.Equals(tempUrl)))
                                {
                                    urls.Add(new ExtractionStatus() { Url = u, Depth = url.Depth + 1, Picked = false });
                                }
                            }
                        }
                        var jobs = _mapper.Map<ScanJobEntity>(scanJob);
                        UpdateCount(jobs, words, wordPairs);
                        jobs.Message = $"{urls.Count(c => c.Picked)} url and dependent urls processed, {urls.Count(c => !c.Picked)} yet to process. ";
                        _storageManager.UpdateScanJobs(jobs);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Some error in extracting content got url {url.Url}", url);
                    }
                }


            }

            scanJob.JobStatus = Common.ScanJobStatus.Completed;
            scanJob.Message = "Extraction done";
            _storageManager.UpdateScanJobs(_mapper.Map<ScanJobEntity>(scanJob));
        }
    }
}

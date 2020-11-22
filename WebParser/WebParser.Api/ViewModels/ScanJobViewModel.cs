using System;
using System.Collections.Generic;
using WebParser.Api.Common;

namespace WebParser.Api
{
    /// <summary>
    /// Top word view model
    /// </summary>
    public class TopWordViewModel
    {
        /// <summary>
        /// String word
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// Count in the web page
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// Word pair view model
    /// </summary>
    public class TopPairViewModel : TopWordViewModel
    {
        /// <summary>
        /// Second word
        /// </summary>
        public string Second { get; set; }

        public string WordPair { get { return $"{Word} {Second}"; } }
    }

    /// <summary>
    /// Scan job view model
    /// </summary>
    public class ScanJobViewModel
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Url to scan
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Top word list. Currently used for top 10
        /// </summary>
        public List<TopWordViewModel> TopWords { get; set; }

        /// <summary>
        /// Top word pair list. Currently used for top 10
        /// </summary>
        public List<TopPairViewModel> TopWordPairs { get; set; }

        /// <summary>
        /// Job Status
        /// </summary>
        public ScanJobStatus JobStatus { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
    }
}

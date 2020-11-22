using System;
using System.Collections.Generic;
using WebParser.Api.Common;

namespace WebParser.Api.Scan
{
    /// <summary>
    /// Top word model
    /// </summary>
    public class TopWord
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
    /// Word pair model
    /// </summary>
    public class TopPair : TopWord
    {
        /// <summary>
        /// Second word
        /// </summary>
        public string Second { get; set; }

        public string WordPair { get { return $"{Word} {Second}"; } }
    }

    /// <summary>
    /// Scan job model
    /// </summary>
    public class ScanJob
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
        public List<TopWord> TopWords { get; set; }

        /// <summary>
        /// Top word pair list. Currently used for top 10
        /// </summary>
        public List<TopPair> TopWordPairs { get; set; }

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

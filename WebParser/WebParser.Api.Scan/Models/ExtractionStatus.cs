namespace WebParser.Api.Scan
{
    /// <summary>
    /// Extraction status class. Temporarily used for tracking progress
    /// </summary>
    public class ExtractionStatus
    {
        /// <summary>
        /// Current url in process
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Depth of the URL being processed
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// If picked for processing
        /// </summary>
        public bool Picked { get; set; }
    }
}

using System.Threading.Tasks;

namespace WebParser.Api.Scan
{
    /// <summary>
    /// Data extractor Interface
    /// </summary>
    public interface IDataExtractor
    {
        /// <summary>
        /// Extract data from webpage
        /// </summary>
        /// <param name="scanJob">Scan job details</param>
        /// <returns>Task</returns>
        Task Extract(ScanJob scanJob);  
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebParser.Api.Scan
{
    /// <summary>
    /// Interface of scan processor
    /// </summary>
    public interface IScanProcessor
    {
        /// <summary>
        /// Enqueue jobs for background processing
        /// </summary>
        /// <param name="url">url to scan</param>
        /// <returns>Initial scan object before scaning is done</returns>
        Task<ScanJob> EnqueueJob(string url);

        /// <summary>
        /// Get all scan jobs 
        /// </summary>
        /// <returns>List of all scan jobs</returns>
        Task<List<ScanJob>> GetScanJobs();

        /// <summary>
        /// Get a scan job by Id 
        /// </summary>
        /// <param name="scanId">Scan job id</param>
        /// <returns>Scan job for given id</returns>
        Task<ScanJob> GetScanJobById(Guid scanId);
    }
}

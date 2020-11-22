using System;
using System.Collections.Generic;
using System.Text;

namespace WebParser.Api.Storage
{
    public interface IStorageManager
    {
        /// <summary>
        /// Add scan data to storage
        /// </summary>
        /// <param name="scanJob">Scan job object</param>
        /// <returns>Persistence status</returns>
        bool AddScanJobs(ScanJobEntity scanJob);

        /// <summary>
        /// Update scan data to storage
        /// </summary>
        /// <param name="scanJob">Scan job object</param>
        /// <returns>Persistence status</returns>
        bool UpdateScanJobs(ScanJobEntity scanJob);

        /// <summary>
        /// Get all scan job from storage
        /// </summary>
        /// <returns>List of all scan jobs</returns>
        List<ScanJobEntity> GetScanJobs();

        /// <summary>
        /// Get a scan job by Id from storage
        /// </summary>
        /// <param name="scanId">Scan job id</param>
        /// <returns>Scan job for given id</returns>
        ScanJobEntity GetScanJobById(Guid scanId);
    }
}

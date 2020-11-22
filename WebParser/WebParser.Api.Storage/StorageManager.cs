using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace WebParser.Api.Storage
{
    /// <summary>
    /// This is an inmemory storage implementation. Currently storing data in memory cache
    /// This can be replaced with any data layer implementation
    /// </summary>
    public class StorageManager : IStorageManager
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<StorageManager> _logger;

        /// <summary>
        /// Private field to keep track of all memory cache keys. Here keys are Job ids
        /// </summary>
        private List<Guid> keys;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="memoryCache">Memory cache</param>
        /// <param name="logger">Logger</param>
        public StorageManager(IMemoryCache memoryCache, ILogger<StorageManager> logger)
        {
            _memoryCache = memoryCache;
            keys = new List<Guid>();
            _logger = logger;
        }

        /// <summary>
        /// Common private method for getting a job from memory cache
        /// </summary>
        /// <param name="jobId">Job id</param>
        /// <returns>Job object</returns>
        private ScanJobEntity GetJobById(Guid jobId)
        {
            ScanJobEntity existingScanJob;
            _memoryCache.TryGetValue(jobId, out existingScanJob);
            return existingScanJob;
        }

        /// <summary>
        /// Add scan data to storage
        /// </summary>
        /// <param name="scanJobEntity">Scan job object</param>
        /// <returns>Persistence status</returns>
        public bool AddScanJobs(ScanJobEntity scanJobEntity)
        {
            ScanJobEntity scanJobExisting = GetJobById(scanJobEntity.Id);
            if (scanJobExisting == null)
            {
                keys.Add(scanJobEntity.Id);
                _memoryCache.Set(scanJobEntity.Id, scanJobEntity);
                return true;
            }
            else
            {
                _logger.LogWarning("Job with this id already exists in memory");
                return false;
            }
        }

        /// <summary>
        /// Update scan data to storage
        /// </summary>
        /// <param name="scanJobEntity">Scan object</param>
        /// <returns>Persistence status</returns>
        public bool UpdateScanJobs(ScanJobEntity scanJobEntity) {
            ScanJobEntity scanJobExisting = GetJobById(scanJobEntity.Id);
            if (scanJobEntity == null)
            {
                _logger.LogWarning("Job with this id should exist in memory");
                return false;
            }
            else {
                _memoryCache.Set(scanJobEntity.Id, scanJobEntity);
                return true;
            }
        }

        /// <summary>
        /// Get a scan job by Id from storage
        /// </summary>
        /// <param name="scanJobId">scan job id</param>
        /// <returns>scan job for given id</returns>
        public ScanJobEntity GetScanJobById(Guid scanJobId)
        {
            return GetJobById(scanJobId);
        }

        /// <summary>
        /// Get all scan jobs from storage
        /// </summary>
        /// <returns>List of all scan jobs</returns>
        public List<ScanJobEntity> GetScanJobs()
        {
            List<ScanJobEntity> jobs = new List<ScanJobEntity>();
            foreach (Guid key in keys)
            {
                var job = GetJobById(key);
                if (job != null)
                {
                    jobs.Add(job);
                }
            }

            return jobs;
        }
    }
}

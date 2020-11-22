using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebParser.Api.BackgroundProcessor;
using WebParser.Api.Common;
using WebParser.Api.Storage;

namespace WebParser.Api.Scan
{
    /// <summary>
    /// Url scan business layer
    /// </summary>
    public class ScanProcessor : IScanProcessor
    {
        private readonly IStorageManager _storageManager;
        private readonly IBackgroundQueue _queue;
        private readonly IMapper _mapper;
        private readonly IDataExtractor _dataExtractor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="storageManager">Storage Manager</param>
        /// <param name="queue">Queue</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="dataExtractor">Data Extractor</param>
        public ScanProcessor(IStorageManager storageManager, 
            IBackgroundQueue queue,
            IMapper mapper,
            IDataExtractor dataExtractor)
        {
            _storageManager = storageManager;
            _queue = queue;
            _mapper = mapper;
            _dataExtractor = dataExtractor;
        }

        /// <summary>
        /// This is the method background worker will be calling to process the job
        /// </summary>
        /// <param name="scanJob">Scan job as input</param>
        /// <returns>task</returns>
        async Task Process(ScanJob scanJob)
        {
            await _dataExtractor.Extract(scanJob);
        }

        /// <summary>
        /// Enqueue jobs for background processing
        /// </summary>
        /// <param name="url">url to scan</param>
        /// <returns>Initial scan object before scaning is done</returns>
        public async Task<ScanJob> EnqueueJob(string url)
        {
            var job = new ScanJobEntity();
            job.Id = Guid.NewGuid();
            job.JobStatus = ScanJobStatus.Pending;
            job.Url = url;
            var jobData = _mapper.Map<ScanJob>(job);
            _storageManager.AddScanJobs(job);
            _queue.QueueItem(async token =>
            {
                await Process(jobData);
            });
            await Task.Yield(); //Faking await inside async method
            return jobData;
        }

        /// <summary>
        /// Get a scan job by Id 
        /// </summary>
        /// <param name="scanId">Scan job id</param>
        /// <returns>Scan job for given id</returns>
        public async Task<ScanJob> GetScanJobById(Guid scanId)
        {
            await Task.Yield(); //Faking await inside async method
            var scan = _storageManager.GetScanJobById(scanId);
            return _mapper.Map<ScanJob>(scan);
        }

        /// <summary>
        /// Get all scan jobs 
        /// </summary>
        /// <returns>List of all scan jobs</returns>
        public async Task<List<ScanJob>> GetScanJobs()
        {
            await Task.Yield(); //Faking await inside async method
            var scanJobs = _storageManager.GetScanJobs();
            if (scanJobs.Count > 0)
            {
                var result = _mapper.Map<List<ScanJob>>(scanJobs);
                return result;
            }
            else {
                return new List<ScanJob>(); 
            }
        }
    }
}

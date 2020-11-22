using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebParser.Api.Common;
using WebParser.Api.Scan;

namespace WebParser.Api.Controllers
{
    /// <summary>
    /// Queue url to scan and get status
    /// </summary>
    [ApiController]
    [Route("scan")]
    public class ScanController : ControllerBase
    {
        private readonly IScanProcessor _scanProcessor;
        private readonly ILogger<ScanController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scanProcessor">Scan processor</param>
        /// <param name="logger">Logger</param>
        /// <param name="mapper">Automapper</param>
        public ScanController(IScanProcessor scanProcessor, ILogger<ScanController> logger, IMapper mapper)
        {
            _scanProcessor = scanProcessor;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Enqueue a url for processing
        /// </summary>
        /// <param name="model">take url as input</param>
        /// <returns>Queued url data</returns>
        [HttpPost]
        public async Task<ActionResult<ScanJobViewModel>> EnqueueJob(ScanJobViewModel model)
        {
            var result = await _scanProcessor.EnqueueJob(model.Url);
            if (result == null)
            {
                _logger.LogInformation("Unable to log Enqueue Job");
                throw new ScanException("Unable to enqueue Scanning job");
            }
            return Ok(result);
        }

        /// <summary>
        /// Get all url scan job from queue with current status
        /// </summary>
        /// <returns>List of scan jobs</returns>
        [HttpGet]
        public async Task<ActionResult<ScanJobViewModel[]>> GetJobs()
        {
            var result = await _scanProcessor.GetScanJobs();
            var jobs = _mapper.Map<List<ScanJobViewModel>>(result);
            return Ok(jobs.ToArray());
        }

        /// <summary>
        /// Get a specific url scan job by id. 
        /// </summary>
        /// <param name="jobId">Job id(Guid)</param>
        /// <returns>Scan job details with current status</returns>
        [HttpGet("{jobId}")]
        public async Task<ActionResult<ScanJobViewModel>> GetJob(Guid jobId)
        {
            if (jobId.Equals(Guid.Empty))
            {
                throw new ScanException("Invalid job id", StatusCodes.Status400BadRequest);
            }
            var result = await _scanProcessor.GetScanJobById(jobId);

            if (result == null)
            {
                throw new ScanException("Job with ID not found", StatusCodes.Status404NotFound);
            }
            else
            {
                return Ok(_mapper.Map<ScanJobViewModel>(result));
            }
        }
    }
}

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebParser.Api.BackgroundProcessor
{
    /// <summary>
    /// Background service implementation based on Microsoft.Extensions.Hosting.BackgroundService
    /// </summary>
    public class BackgroundQueueService : BackgroundService
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Takes background queue and logger 
        /// </summary>
        /// <param name="queue">Queue</param>
        /// <param name="loggerFactory">Logger</param>
        public BackgroundQueueService(IBackgroundQueue queue,
            ILoggerFactory loggerFactory)
        {
            Queue = queue;
            _logger = loggerFactory.CreateLogger<BackgroundQueueService>();
        }

        /// <summary>
        /// Queue
        /// </summary>
        public IBackgroundQueue Queue { get; }


        /// <summary>
        /// Overloaded Queue execution per item
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async override Task ExecuteAsync(
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Queued Hosted Service is starting for item processing");

            while (!cancellationToken.IsCancellationRequested)
            {
                var workItem = await Queue.DequeueAsync(cancellationToken);

                try
                {
                    await workItem(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                       "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }

            _logger.LogInformation("Queued Hosted Service is stopping after item processing");
        }
    }

}

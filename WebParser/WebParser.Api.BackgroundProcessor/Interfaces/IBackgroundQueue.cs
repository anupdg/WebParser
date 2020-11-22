using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebParser.Api.BackgroundProcessor
{
    /// <summary>
    /// Interface for queue implementation
    /// </summary>
    public interface IBackgroundQueue
    {
        /// <summary>
        /// Queue a job which to be processed later
        /// </summary>
        /// <param name="workItem">Queue item processing lambda</param>
        void QueueItem(Func<CancellationToken, Task> workItem);

        /// <summary>
        /// Dequeue item for processing
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns lambda to be processed</returns>
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace WebParser.Api.BackgroundProcessor
{
    /// <summary>
    /// Implementation for IBackgroundQueue
    /// </summary>
    public class BackgroundQueue : IBackgroundQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task>> _workItems =
               new ConcurrentQueue<Func<CancellationToken, Task>>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        /// <summary>
        /// Queue a job which to be processed later
        /// </summary>
        /// <param name="workItem">Queue item processing lambda</param>
        public void QueueItem(
            Func<CancellationToken, Task> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _workItems.Enqueue(workItem);
            _signal.Release();
        }

        /// <summary>
        /// Dequeue item for processing
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns lambda to be processed</returns>
        public async Task<Func<CancellationToken, Task>> DequeueAsync(
            CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);
            if (workItem == null)
            {
                //This exception is done purposefully, in ideal case this should never be null.
                throw new Exception("Work items cannot be null");
            }
            else
            {
                return workItem;
            }
        }
    }
}

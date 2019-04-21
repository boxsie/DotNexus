using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNexus.Jobs
{
    public abstract class HostedService : IHostedService
    {
        public bool IsRunning => _executingTask != null && _executingTask.IsCompleted;

        protected readonly ILogger<HostedService> Logger;

        private TimeSpan _jobInterval;
        private Task _executingTask;
        private CancellationTokenSource _cts;

        protected HostedService(ILogger<HostedService> logger)
        {
            Logger = logger;
            _jobInterval = TimeSpan.FromSeconds(1);
        }

        protected abstract Task ExecuteAsync();

        public virtual Task StartAsync(TimeSpan interval, CancellationToken token)
        {
            _jobInterval = interval;

            return StartAsync(token);
        }

        public virtual Task StartAsync(CancellationToken token)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(token);

            _executingTask = ExecuteIntervalAsync(_cts.Token);

            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken token)
        {
            if (_executingTask == null)
                return;

            _cts.Cancel();

            await Task.WhenAny(_executingTask, Task.Delay(-1, token));

            token.ThrowIfCancellationRequested();
        }

        private async Task ExecuteIntervalAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await ExecuteAsync();
                }
                catch (Exception e)
                {
                    Logger.LogCritical(e.Message);
                    Logger.LogCritical(e.StackTrace);
                    throw;
                }

                if (_jobInterval == TimeSpan.Zero)
                    await StopAsync(cancellationToken);
                else
                    await Task.Delay(_jobInterval, cancellationToken);
            }
        }
    }
}
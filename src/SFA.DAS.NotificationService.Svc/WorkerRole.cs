using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using NLog;
using StructureMap;

namespace SFA.DAS.NotificationService.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private Container _container;

        public override void Run()
        {
            Logger.Info("Running");
            Trace.TraceInformation("SFA.DAS.NotificationService.Worker is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw;
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            Logger.Info("Starting");

            var registry = new DefaultRegistry();

            _container = new Container(registry);

            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("SFA.DAS.NotificationService.Worker has been started");

            return result;
        }

        public override void OnStop()
        {
            Logger.Info("Stopping");

            Trace.TraceInformation("SFA.DAS.NotificationService.Worker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("SFA.DAS.NotificationService.Worker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            var handler = _container.GetInstance<QueuedMessageHandler>();

            while (!cancellationToken.IsCancellationRequested)
            {
                Logger.Debug("Polling");
                try
                {
                    handler.Handle();

                    await Task.Delay(1000);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, ex.Message);
                }
            }
        }
    }
}

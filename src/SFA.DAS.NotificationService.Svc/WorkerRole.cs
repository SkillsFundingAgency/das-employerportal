using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.WindowsAzure.ServiceRuntime;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Application;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;
using SFA.DAS.NotificationService.Application.Queries.GetMessage;
using StructureMap;

namespace SFA.DAS.NotificationService.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private Container _container;

        public override void Run()
        {
            Trace.TraceInformation("SFA.DAS.NotificationService.Svc is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            var registry = new DefaultRegistry();

            _container = new Container(registry);

            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("SFA.DAS.NotificationService.Svc has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("SFA.DAS.NotificationService.Svc is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("SFA.DAS.NotificationService.Svc has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");

                var handler = _container.GetInstance<QueuedMessageHandler>();

                handler.Handle();

                await Task.Delay(1000);
            }
        }
    }
}

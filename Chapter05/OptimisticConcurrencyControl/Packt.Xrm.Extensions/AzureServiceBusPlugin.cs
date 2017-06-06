using System;
using Microsoft.Xrm.Sdk;

namespace OptimisticConcurrencyControl
{
    public class AzureServiceBusPlugin : IPlugin
    {
        private Guid serviceEndpointId;

        public AzureServiceBusPlugin(string config)
        {
            if (string.IsNullOrEmpty(config) || !Guid.TryParse(config, out serviceEndpointId))
            {
                throw new InvalidPluginExecutionException("Service endpoint ID should be passed as config.");
            }
            //d881b3a8-1ecc-e611-80f4-c4346bc4beac
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            // Retrieve the execution context.
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Extract the tracing service.
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

var cloudService = (IServiceEndpointNotificationService)serviceProvider.GetService(typeof(IServiceEndpointNotificationService));
if (cloudService == null)
    throw new InvalidPluginExecutionException("Failed to retrieve the service bus service.");
string response = cloudService.Execute(new EntityReference("serviceendpoint", serviceEndpointId), context);

            try
            {
                tracingService.Trace("Posting the execution context.");
                if (!string.IsNullOrEmpty(response))
                {
                    tracingService.Trace("Response = {0}", response);
                }
                tracingService.Trace("Done.");
            }
            catch (Exception e)
            {
                tracingService.Trace("Exception: {0}", e.ToString());
                throw;
            }
        }
    }
}

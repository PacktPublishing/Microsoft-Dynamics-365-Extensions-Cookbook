using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Packt.Xrm.Entities;
using Microsoft.Crm.Sdk.Messages;
using System.ServiceModel;

namespace Packt.Xrm.Extensions
{
    public class BlankActivity : IPlugin
    {      
        public void Execute(IServiceProvider serviceProvider)
        {
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            tracingService.Trace("contains tager? {0}", context.InputParameters.Contains("Target"));

            if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity))
                return;

            Entity entity = (Entity)context.InputParameters["Target"];
            tracingService.Trace("entity logical Name? {0}", entity.LogicalName);

            if (entity.LogicalName != "account")
                return;

            tracingService.Trace("Before try");

            try
            {                
                tracingService.Trace("Activity that does nothing");
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in the UpdateActivities plug-in.", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("FollowupPlugin: {0}", ex.ToString());
                throw;
            }
        }
    }
}

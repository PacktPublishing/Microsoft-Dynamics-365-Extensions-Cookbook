using Microsoft.Xrm.Sdk;
using Packt.Xrm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Xrm.Refactored.Plugin
{
    public abstract class BaseAzureAwarePlugin<T>: BasePlugin where T : Entity//<T> : BasePlugin/*<T>*/ where T : Entity
    {
        T GenericEntity;

        protected Guid _serviceEndpointId;
        public override string ExpectedEntityLogicalName { get { return GenericEntity.LogicalName; } }
        
        //public BaseAzureAwarePlugin(string config)
        //{
        //    if (string.IsNullOrEmpty(config) || !Guid.TryParse(config, out _serviceEndpointId))
        //    {
        //        throw new InvalidPluginExecutionException("Service endpoint ID should be passed as config.");
        //    }
        //}
        //public override void PostExecute(IServiceProvider serviceProvider)
        //{
        //    //var cloudService = (IServiceEndpointNotificationService)serviceProvider.GetService(typeof(IServiceEndpointNotificationService));
        //    //if (cloudService == null)
        //    //    throw new InvalidPluginExecutionException("Failed to retrieve the service bus service.");

        //    //var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));


        //    //CustomTracingService = DataAccessLayerFactory.GetAzureAwareTracingService(cloudService, _serviceEndpointId, context);

        //    PostAzureExecute(serviceProvider);
        //}

        //public abstract void PostAzureExecute(IServiceProvider serviceProvider);
    }
}

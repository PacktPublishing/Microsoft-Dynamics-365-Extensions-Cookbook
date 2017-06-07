using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using Packt.Xrm.Refactored.DataAccessLayer;

namespace Packt.Xrm.Refactored.Plugin
{
    public abstract class BasePlugin : IPlugin//<T> : IPlugin where T : Entity
    {
        protected IOrganizationService OrganizationService;
        protected IPluginExecutionContext PluginContext;
        protected ICustomTracingService CustomTracingService;
        protected Entity Entity;
        //protected string EntityType;
        protected DataAccessLayerFactory DataAccessLayerFactory;

        public abstract void PostExecute(IServiceProvider serviceProvider);
        //public abstract string GetExpectedEntityLogicalName();
        public abstract string ExpectedEntityLogicalName { get; }

        public void Execute(IServiceProvider serviceProvider)
        {
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            PluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (!PluginContext.InputParameters.Contains("Target") || !(PluginContext.InputParameters["Target"] is Entity))
                return;

            Entity = (Entity)PluginContext.InputParameters["Target"];

            if (Entity.LogicalName != ExpectedEntityLogicalName)
                return;

            //Entity = genericEntity.ToEntity();
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            OrganizationService = serviceFactory.CreateOrganizationService(PluginContext.UserId);

            using (DataAccessLayerFactory = new DataAccessLayerFactory(OrganizationService, tracingService))
            {
                CustomTracingService = DataAccessLayerFactory.GetTracingService();
                try
                {
                    PostExecute(serviceProvider);
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in this plug-in.", ex);
                }
                catch (Exception ex)
                {
                    CustomTracingService.Trace("Plugin Exception: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}

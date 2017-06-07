using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
//using Packt.Xrm.Entities;
//using Microsoft.Crm.Sdk.Messages;
using System.ServiceModel;
using Microsoft.Xrm.Sdk.Workflow;
using Packt.Xrm.Refactored.DataAccessLayer;
using System.Activities;

namespace Packt.Xrm.Refactored.CustomWorfklow
{
    public abstract class BaseCustomWorkflow : CodeActivity
    {
        protected IOrganizationService OrganizationService;
        protected ICustomTracingService CustomTracingService;
        protected DataAccessLayerFactory DataAccessLayerFactory;
        protected IWorkflowContext WorkflowContext;

        public abstract void PostExecute(CodeActivityContext executionContext);

        protected override void Execute(CodeActivityContext executionContext)
        {
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var tracingService = executionContext.GetExtension<ITracingService>();
            WorkflowContext = executionContext.GetExtension<IWorkflowContext>();
            OrganizationService = serviceFactory.CreateOrganizationService(WorkflowContext.UserId);

            try
            {
                using (DataAccessLayerFactory = new DataAccessLayerFactory(OrganizationService, tracingService))
                {
                    CustomTracingService = DataAccessLayerFactory.GetTracingService();

                    PostExecute(executionContext);

                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in the UpdateActivities plug-in.", ex);
            }
            catch (Exception ex)
            {
                CustomTracingService.Trace("FollowupPlugin: {0}", ex.ToString());
                throw;
            }
        }

    }
}


//var input = AccountGuid.Get<string>(executionContext);
//accountId = input == Guid.Empty.ToString().Trim('{').Trim('}') ? context.PrimaryEntityId : Guid.Parse(input);
//tracingService.Trace("Input value {0}", input);

//var counterClosed = 0;
//var counterUpdated = 0;
//var emails = GetEmails(accountId);
//foreach (var email in emails)
//{
//    if (string.IsNullOrEmpty(email.Subject))
//    {
//        CloseEmailAsCancelled(email);
//        counterClosed++;
//    }
//    else
//    {
//        email.ScheduledStart = DateTime.Today.AddDays(10);
//        UpdateEntity(email);
//        counterUpdated++;
//    }
//}

//Commit();
//tracingService.Trace("{0} Closed", counterClosed);
//tracingService.Trace("{0} Updated", counterUpdated);
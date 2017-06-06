using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Packt.Xrm.Entities;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.ServiceBus.Messaging;


namespace Packt.Xrm.Extensions
{
    public class UpdateActivities_ReusedASB : IPlugin
    {
        IOrganizationService _organizationService;
        OrganisationServiceContext _organizationContext;

        private Guid serviceEndpointId;

        public UpdateActivities_ReusedASB(string config)
        {
            if (string.IsNullOrEmpty(config) || !Guid.TryParse(config, out serviceEndpointId))
            {
                throw new InvalidPluginExecutionException("Service endpoint ID should be passed as config.");
            }

        }

        public void Execute(IServiceProvider serviceProvider)
        {
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity))
                return;

            Entity entity = (Entity)context.InputParameters["Target"];

            if (entity.LogicalName != "account")
                return;

            try
            {
                var cloudService = (IServiceEndpointNotificationService)serviceProvider.GetService(typeof(IServiceEndpointNotificationService));
                if (cloudService == null)
                    throw new InvalidPluginExecutionException("Failed to retrieve the service bus service.");
                string response = cloudService.Execute(new EntityReference("serviceendpoint", serviceEndpointId), context);


                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                _organizationService = serviceFactory.CreateOrganizationService(context.UserId);

                _organizationContext = new OrganisationServiceContext(_organizationService);

                var emails = GetEmails(entity.Id);
                foreach (var email in emails)
                {
                    if (string.IsNullOrEmpty(email.Subject))
                    {
                        CloseEmailAsCancelled(email);
                    }
                    else
                    {
                        email.ScheduledStart = DateTime.Today.AddDays(10);
                        UpdateEntity(email);
                    }
                }

                Commit();
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in the FollowupPlugin plug-in.", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("FollowupPlugin: {0}", ex.ToString());
                throw;
            }
        }

        public IEnumerable<Email> GetEmails(Guid parentEntityId)
        {
            var query = from email in _organizationContext.EmailSet
                        where email.RegardingObjectId.Id == parentEntityId
                         &&
                         email.StateCode == EmailState.Open
                        select new Email
                        {
                            Id = email.Id,
                            Subject = email.Subject
                        };


            return query.ToList();
        }

        public void CloseEmailAsCancelled(Email email)
        {
            email.StateCode = EmailState.Canceled;

            var setStateRequest = new SetStateRequest()
            {
                Status = new OptionSetValue((int)email_statuscode.Canceled),
                State = new OptionSetValue((int)EmailState.Canceled),
                EntityMoniker = new EntityReference(Email.EntityLogicalName, email.Id)
            };
            _organizationContext.Execute(setStateRequest);
        }

        public void UpdateEntity(Entity entity)
        {
            _organizationContext.UpdateObject(entity);
        }

        public void Commit()
        {
            _organizationContext.SaveChanges();
        }

    }
}

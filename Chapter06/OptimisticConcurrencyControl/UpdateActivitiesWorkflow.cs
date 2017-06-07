using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Packt.Xrm.Entities;
using Microsoft.Crm.Sdk.Messages;
using System.ServiceModel;

//using Microsoft.Crm.Sdk.Messages;

//using Microsoft.Xrm.Sdk;
//using Microsoft.Xrm.Sdk.Messages;
//using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace OptimisticConcurrencyControl
{
    public class UpdateActivitiesWorkflow : CodeActivity
    {

        OrganisationServiceContext _organizationContext;
        IOrganizationService _organizationService;


        Guid emailGuid1;
        Guid emailGuid2;
        Guid emailGuid3;
        Guid accountId;

        [Input("Account GUID")]
        [Default("{575A8B41-F8D7-4DCE-B2EA-3FFDE936AB1B}")]
        public InArgument<string> AccountGuid { get; set; }


        public void SetOrganisationServiceProxy(IOrganizationService organisationService)
        {
            _organizationService = organisationService;
            _organizationContext = new OrganisationServiceContext(_organizationService);
        }
        public Guid createAccountAndEmails()
        {
            var account = new Account()
            {
                Name = "packt 365"
            };

            accountId = _organizationService.Create(account);

            var email1 = new Email()
            {
                Subject = "Packt 1",
                ScheduledStart = DateTime.Today.AddDays(-3),
                RegardingObjectId = new EntityReference(Account.EntityLogicalName, accountId)
            };
            var email2 = new Email()
            {
                Subject = "Packt 2",
                ScheduledStart = DateTime.Today.AddDays(-3),
                RegardingObjectId = new EntityReference(Account.EntityLogicalName, accountId)
            };
            var email3 = new Email()
            {
                Subject = "Packt 3",
                Description = "Hello World 123",
                ScheduledStart = DateTime.Today.AddDays(-3),
                RegardingObjectId = new EntityReference(Account.EntityLogicalName, accountId)
            };

            emailGuid1 = _organizationService.Create(email1);
            emailGuid2 = _organizationService.Create(email2);
            emailGuid3 = _organizationService.Create(email3);

            return accountId;
        }

        public void deleteAll()
        {
            _organizationService.Delete(Email.EntityLogicalName, emailGuid1);
            _organizationService.Delete(Email.EntityLogicalName, emailGuid2);
            _organizationService.Delete(Email.EntityLogicalName, emailGuid3);
            _organizationService.Delete(Account.EntityLogicalName, accountId);
        }

        public IEnumerable<Email> GetEmails(Guid parentEntityId)
        {
            _organizationContext = new OrganisationServiceContext(_organizationService);

            var query = from email in _organizationContext.EmailSet
                        where email.RegardingObjectId.Id == parentEntityId
                        &&
                        email.StateCode == EmailState.Open
                        select new Email
                        {
                            Id = email.Id,
                            Description = email.Description
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

        protected override void Execute(CodeActivityContext executionContext)
        {
            //var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            //var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            //if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity))
            //    return;

            //Entity entity = (Entity)context.InputParameters["Target"];

            //if (entity.LogicalName != "account")
            //    return;

            //IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            //_organizationService = serviceFactory.CreateOrganizationService(context.UserId);

        var tracingService = executionContext.GetExtension<ITracingService>();

        //Create the context
        var context = executionContext.GetExtension<IWorkflowContext>();
        var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
        _organizationService = serviceFactory.CreateOrganizationService(context.UserId);


            try
            {
                var accountId = Guid.Parse(AccountGuid.Get<string>(executionContext));

                _organizationContext = new OrganisationServiceContext(_organizationService);

                var emails = GetEmails(accountId);
                foreach (var email in emails)
                {
                    if (string.IsNullOrEmpty(email.Description))
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

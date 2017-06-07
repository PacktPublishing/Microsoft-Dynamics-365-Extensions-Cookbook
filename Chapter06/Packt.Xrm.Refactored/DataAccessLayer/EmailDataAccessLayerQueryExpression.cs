using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Packt.Xrm.Entities;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;

namespace Packt.Xrm.Refactored.DataAccessLayer
{
    public class EmailDataAccessLayerQueryExpression : BaseDataAccessLayer, IEmailDataAccessLayer
    {
        public EmailDataAccessLayerQueryExpression(IOrganizationService organizationService) : base(organizationService)
        {
        }

        public override void UpdateEntity(Entity entity)
        {
            OrganizationService.Update(entity);
        }

        public override void Commit()
        {
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
            OrganizationService.Execute(setStateRequest);
        }

public IEnumerable<Email> GetEmails(Guid parentEntityId)
{
    var queryExpression = new QueryExpression()
    {
        EntityName = Email.EntityLogicalName,
        ColumnSet = new ColumnSet( "subject"),
        Criteria =
        {
            Filters =
                {
                    new FilterExpression
                    {
                        FilterOperator = LogicalOperator.And,
                        Conditions =
                        {
                            new ConditionExpression("regardingobjectid", ConditionOperator.Equal, parentEntityId),
                            new ConditionExpression("statecode", ConditionOperator.Equal, (int)EmailState.Open)
                        },
                    }
                }
        }
    };

    var entityCollection = OrganizationService.RetrieveMultiple(queryExpression);
    return entityCollection.Entities.Select(e => e.ToEntity<Email>()).ToList();
}
    }
}

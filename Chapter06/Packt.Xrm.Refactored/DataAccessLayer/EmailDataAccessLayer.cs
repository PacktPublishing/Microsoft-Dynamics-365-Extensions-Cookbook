using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packt.Xrm.Entities;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace Packt.Xrm.Refactored.DataAccessLayer
{
    public class EmailDataAccessLayer : BaseDataAccessLayer, IEmailDataAccessLayer
    {
        public EmailDataAccessLayer(IOrganizationService organizationService) : base(organizationService)
        {
        }

        public IEnumerable<Email> GetEmails(Guid parentEntityId)
        {
            var query = from email in OrganizationContext.EmailSet
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
            OrganizationContext.Execute(setStateRequest);
        }
    }
}

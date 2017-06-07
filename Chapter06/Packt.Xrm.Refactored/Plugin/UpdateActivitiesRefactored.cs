using Packt.Xrm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Packt.Xrm.Refactored.DataAccessLayer;
using Packt.Xrm.Refactored.BusinessLogic;

namespace Packt.Xrm.Refactored.Plugin
{
    public class UpdateActivitiesRefactored : BasePlugin//BaseAzureAwarePlugin<Account>
    {
        public override string ExpectedEntityLogicalName { get { return Account.EntityLogicalName; } }

        public override void PostExecute()
        {
            var updateEmailLogic = new UpdateEmailLogic(DataAccessLayerFactory.GetEmailDataAccessLayer(), CustomTracingService);
            updateEmailLogic.UpdateAccountsEmails(Entity.Id);
        }
    }
}

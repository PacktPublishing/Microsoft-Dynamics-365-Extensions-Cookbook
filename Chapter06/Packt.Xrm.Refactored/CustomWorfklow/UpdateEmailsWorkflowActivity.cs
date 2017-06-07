using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Packt.Xrm.Refactored.BusinessLogic;

namespace Packt.Xrm.Refactored.CustomWorfklow
{
     class UpdateEmailsWorkflowActivity : BaseCustomWorkflow
    {
[Input("Account Guid")]
[Default("00000000-0000-0000-0000-000000000000")]
public InArgument<string> AccountGuid { get; set; }

public override void PostExecute(CodeActivityContext executionContext)
{
    var input = AccountGuid.Get<string>(executionContext);
    var accountId = input == Guid.Empty.ToString().Trim('{').Trim('}') ? WorkflowContext.PrimaryEntityId : Guid.Parse(input);
    CustomTracingService.Trace("Input value {0}", input);

    var updateEmailLogic = new UpdateEmailLogic(DataAccessLayerFactory.GetEmailDataAccessLayer(), CustomTracingService);
    updateEmailLogic.UpdateAccountsEmails(accountId);
}
    }
}

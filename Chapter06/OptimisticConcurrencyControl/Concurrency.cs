using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Deployment;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using UpdateRequest = Microsoft.Xrm.Sdk.Messages.UpdateRequest;
using UpdateResponse = Microsoft.Xrm.Sdk.Messages.UpdateResponse;

namespace OptimisticConcurrencyControl
{
    class Concurrency
    {
        //private Guid _accountId;
        //private string _accountRowVersion;
        //private static OrganizationServiceProxy _serviceProxy;

        static void Main(string[] args)
        {
            var connectionString = "AuthType=Office365;Username=ramim@.onmicrosoft.com; Password=;Url=https://.crm6.dynamics.com";

            var crmSvc = new CrmServiceClient(connectionString);

            using (var serviceProxy = crmSvc.OrganizationServiceProxy)
            {
                
                var accountToCreate = new Entity("account");
                accountToCreate["name"] = "Packt V1.0";

                //Create an account
                var accountGuid = serviceProxy.Create(accountToCreate);


                // Retrieve the account
                var account = serviceProxy.Retrieve("account", accountGuid, new ColumnSet("name"));
                Console.WriteLine("The account version is {0}", account.RowVersion);

                account["name"] = "Packt v2.0";

                // Built the request 
                UpdateRequest accountUpdate = new UpdateRequest()
                {
                    Target = account,
                    ConcurrencyBehavior = ConcurrencyBehavior.IfRowVersionMatches
                };

                // Update the account
                UpdateResponse accountUpdateResponse1 = (UpdateResponse)serviceProxy.Execute(accountUpdate);


                account["name"] = "Packt v3.0";

                // Update the account again
                UpdateResponse accountUpdateResponse2 = (UpdateResponse)serviceProxy.Execute(accountUpdate);

                serviceProxy.Delete("account", accountGuid);
            }
            //</snippetUpdateAndDelete1> 
        }



    }
}


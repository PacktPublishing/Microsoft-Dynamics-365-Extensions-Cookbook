using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System.ServiceModel;

using Microsoft.Xrm.Sdk.Deployment;


namespace OptimisticConcurrencyControl
{
    class Properties
    {
        static void Main()
        {
            var connectionString = "AuthType=Office365;Username=ramim@.onmicrosoft.com; Password=;Url=https://.crm6.dynamics.com";

            var crmSvc = new CrmServiceClient(connectionString);

            var settings = new ImportSettings();

                


            using (var serviceProxy = crmSvc.OrganizationServiceProxy)
            {
                //Create request collection
                var request = new OrganizationRequestCollection()
    {
            new CreateRequest
            {
                Target = new Entity("account")
                {
                    ["name"] = "Packt Account"
                }
            },
            new CreateRequest
            {
                Target = new Entity("contact")
                {
                    ["firstname"] = "Packt",
                    ["lastname"] = "Contact"
                }
            }
    };

                //Create Transaction and pass previously created request collection
                var requestToCreateRecords = new ExecuteMultipleRequest()
                {
                    // Create an empty organization request collection.
                    Requests = request,
                    Settings = new ExecuteMultipleSettings()
                    {
                        ContinueOnError = false,
                        ReturnResponses = true
                    }
                };

                //Execute requests within the transaction
                var responseForCreateRecords = (ExecuteMultipleResponse)serviceProxy.Execute(requestToCreateRecords);

                // Display the results of each response.
                foreach (var responseItem in responseForCreateRecords.Responses)
                {
                    Console.WriteLine("Created record with GUID {0}", responseItem.Response.Results["id"].ToString());
                }
            }
        }
    }
}


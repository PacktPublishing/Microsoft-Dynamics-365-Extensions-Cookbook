using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using Packt.Xrm.Entities;

namespace OptimisticConcurrencyControl
{
    class DAL
    {
        public DAL()
        {
            var connectionString = "AuthType=Office365;Username=@.onmicrosoft.com;Password=;Url=https://.crm6.dynamics.com";

            var crmSvc = new CrmServiceClient(connectionString);

            
            using (var serviceProxy = crmSvc.OrganizationServiceProxy)
            {
                serviceProxy.EnableProxyTypes();
                using (var organisationContext = new OrganisationServiceContext(serviceProxy))
                {
                    //Your code goes here
                }
            }


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using Packt.Xrm.Entities;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Packt.Xrm.Extensions
{
    class Dynamics365DataAccessLayer
    {
        private CrmServiceClient _crmSvc;

        public Dynamics365DataAccessLayer()
        {
            var connectionString = "AuthType=Office365;Username=@.onmicrosoft.com;Password=;Url=https://.crm6.dynamics.com";
            _crmSvc = new CrmServiceClient(connectionString);

        }

        public void Connect()
        {
            using (var serviceProxy = _crmSvc.OrganizationServiceProxy)
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

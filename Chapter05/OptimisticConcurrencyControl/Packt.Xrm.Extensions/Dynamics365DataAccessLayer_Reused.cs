using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using Packt.Xrm.Entities;

namespace Packt.Xrm.Extensions
{
    public class Dynamics365DataAccessLayer_Reused
    {
        private CrmServiceClient _crmSvc;

        public Dynamics365DataAccessLayer_Reused()
        {
            var connectionString = "AuthType=Office365;Username=@.onmicrosoft.com;Password=;Url=https://.crm6.dynamics.com";
            _crmSvc = new CrmServiceClient(connectionString);
        }

        public Dynamics365DataAccessLayer_Reused(string connectionString)
        {
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

        public void CreateAccount()
        {
            using (var serviceProxy = _crmSvc.OrganizationServiceProxy)
            {
                serviceProxy.Create(new Account
                {
                    Name = string.Format("MVC account {0}", DateTime.UtcNow)
                });
            }
        }


    }
}

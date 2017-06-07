using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

namespace Packt.Xrm.Refactored.DataAccessLayer
{
    public class AzureCrmTracing : ICustomTracingService
    {
        IOrganizationService _organizationService;
        ITracingService _tracing;

        public AzureCrmTracing(IOrganizationService organizationService, ITracingService tracing)
        {
            _organizationService = organizationService;
            _tracing = tracing;
        }

        public void Trace(string message, params object[] args)
        {
            _tracing.Trace(message, args);

            var entity = new Entity("packt_log");
            entity.Attributes["packt_logdetails"] = string.Format(message, args);
            _organizationService.Create(entity);
        }
    }
}

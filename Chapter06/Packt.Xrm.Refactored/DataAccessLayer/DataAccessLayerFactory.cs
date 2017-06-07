using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Xrm.Refactored.DataAccessLayer
{
    public class DataAccessLayerFactory : IDisposable
    {
        private IOrganizationService _organisationService;
        private IEmailDataAccessLayer _emailDataAccessLayer;
        private ICustomTracingService _customTracingService;
        private ITracingService _tracingService;

        public DataAccessLayerFactory(IOrganizationService organisationService, ITracingService tracingService)
        {
            _organisationService = organisationService;
            _tracingService = tracingService;
        }
        public IEmailDataAccessLayer GetEmailDataAccessLayer()
        {
            if (_emailDataAccessLayer == null)
                _emailDataAccessLayer = new EmailDataAccessLayer(_organisationService);
                

            return _emailDataAccessLayer;
        }

public ICustomTracingService GetTracingService()
{
    if (_customTracingService == null)
        _customTracingService = new CrmTracing(_tracingService);

    return _customTracingService;
}

        //public ICustomTracingService GetAzureAwareTracingService(IServiceEndpointNotificationService cloudService, Guid serviceEndpointId, IPluginExecutionContext context)
        //{
        //    if (_customTracingService == null)
        //        _customTracingService = new AzureCrmTracing(cloudService, serviceEndpointId, context);
            
        //    return _customTracingService;
        //}

        public void Dispose()
        {
            if (_emailDataAccessLayer != null)
                _emailDataAccessLayer.Dispose();
        }
    }
}

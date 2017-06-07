using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Packt.Xrm.Entities;

namespace Packt.Xrm.Refactored.DataAccessLayer
{

    public class BaseDataAccessLayer : IBaseDataAccessLayer, IDisposable
    {
        protected IOrganizationService OrganizationService;
        protected OrganisationServiceContext OrganizationContext;

        public BaseDataAccessLayer(IOrganizationService organizationService)
        {
            OrganizationService = organizationService;
            OrganizationContext = new OrganisationServiceContext(OrganizationService);
        }
        public virtual void UpdateEntity(Entity entity)
        {
            OrganizationContext.UpdateObject(entity);
        }

        public virtual void Commit()
        {
            OrganizationContext.SaveChanges();
        }

        public virtual void Dispose()
        {
            OrganizationContext.Dispose();
        }
    }
}

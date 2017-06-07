using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace Packt.Xrm.Refactored.DataAccessLayer
{
    public interface IBaseDataAccessLayer
    {
        void UpdateEntity(Entity entity);
        void Commit();
    }
}

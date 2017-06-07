using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packt.Xrm.Entities;

namespace Packt.Xrm.Refactored.DataAccessLayer
{
    public interface IEmailDataAccessLayer : IBaseDataAccessLayer, IDisposable
    {
        IEnumerable<Email> GetEmails(Guid parentEntityId);
        void CloseEmailAsCancelled(Email email);
    }
}

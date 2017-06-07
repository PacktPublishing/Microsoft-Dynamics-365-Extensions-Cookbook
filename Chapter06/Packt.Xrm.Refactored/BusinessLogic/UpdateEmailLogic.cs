using Packt.Xrm.Refactored.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Xrm.Refactored.BusinessLogic
{
    public class UpdateEmailLogic
    {
        private IEmailDataAccessLayer _emailDataAccessLayer;
        private ICustomTracingService _tracingService;

        public UpdateEmailLogic(IEmailDataAccessLayer emailDataAccessLayer, ICustomTracingService tracingService)
        {
            _emailDataAccessLayer = emailDataAccessLayer;
            _tracingService = tracingService;
        }

        public void UpdateAccountsEmails(Guid accountId)
        {
            var emails = _emailDataAccessLayer.GetEmails(accountId);
            int closedEmails = 0;
            int updatedEmails = 0;
            foreach (var email in emails)
            {
                if (string.IsNullOrEmpty(email.Subject))
                {
                    _emailDataAccessLayer.CloseEmailAsCancelled(email);
                    closedEmails++;
                }
                else
                {
                    email.ScheduledStart = DateTime.Today.AddDays(10);
                    _emailDataAccessLayer.UpdateEntity(email);
                    updatedEmails++;
                }
            }
            _tracingService.Trace("{0} closed emails and {1} updated emails", closedEmails, updatedEmails);
            _emailDataAccessLayer.Commit();
        }

    }
}

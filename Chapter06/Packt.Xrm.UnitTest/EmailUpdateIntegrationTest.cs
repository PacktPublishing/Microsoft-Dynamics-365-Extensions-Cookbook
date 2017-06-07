using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Configuration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Packt.Xrm.Entities;
using Microsoft.Xrm.Sdk.Query;
using Packt.Xrm.Refactored.DataAccessLayer;
using Packt.Xrm.Refactored.BusinessLogic;
using Moq;

namespace Packt.Xrm.UnitTest
{
    [TestClass]
    public class EmailUpdateIntegrationTest
    {
        private IOrganizationService _service;
        private Guid? _accountGuid;
        private Guid? _email1Guid;
        private Guid? _email2Guid;
        private Guid? _email3Guid;
        private const int NumberOfDaysDifference = 10;

        public void EstablishConnection()
        {
            var connectionString = ConfigurationManager.AppSettings["Dynamics365ConnectionString"];
            var _crmSvc = new CrmServiceClient(connectionString);

            _service = _crmSvc.OrganizationServiceProxy;
            //_service.EnableProxyTypes();
        }

[TestMethod]
[TestCategory("ConnectedBusinessLogic")]
public void Connection()
{
    //Arrange
    EstablishConnection();

    var _customTracing = new Mock<ICustomTracingService>();
    var _emailDataAccessLayer = new EmailDataAccessLayerQueryExpression(_service);

    var businessLogic = new UpdateEmailLogic(_emailDataAccessLayer, _customTracing.Object);

    //Act
    businessLogic.UpdateAccountsEmails(new Guid("test"));

    //Assert
    //TODO check that the emails were updated
}

        //[ClassInitialize]
        [ClassCleanup]
        public static void DeleteCreatedRecords()
        {
            string test = "Nothing to do";
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Integration()
        {
            //Arrange
            EstablishConnection();
            CreateRecords();

            //Act
            var account = new Account()
            {
                Id = _accountGuid.Value,
                Name = "Sample Test Updated"
            };
            _service.Update(account);

            //Assert
            var email1 = RetrieveEmail(_email1Guid.Value);
            Assert.AreEqual(email1.StateCode, EmailState.Open);
            Assert.AreEqual(email1.ScheduledStart, DateTime.Now.Date.AddDays(NumberOfDaysDifference - 1).ToUniversalTime(), "Start date not updated correctly for emails with a subject");

            var email2 = RetrieveEmail(_email2Guid.Value);
            Assert.AreEqual(email2.StateCode, EmailState.Open);
            Assert.AreEqual(email2.ScheduledStart, DateTime.Now.Date.AddDays(NumberOfDaysDifference - 1).ToUniversalTime(), "Start date not updated correctly for emails with a subject");

            var email3 = RetrieveEmail(_email3Guid.Value);
            Assert.AreEqual(email3.StateCode, EmailState.Canceled, "Email without a subject was not cancelled");
        }

        private Email RetrieveEmail(Guid emailGuid)
        {
            return _service.Retrieve(Email.EntityLogicalName, emailGuid, new ColumnSet("scheduledstart", "statecode")).ToEntity<Email>();
        }

        private void CreateRecords()
        {
            var account = new Account()
            {
                Name = "Sample Test"
            };

            _accountGuid = _service.Create(account);

            _email1Guid = _service.Create(CreateEmail(_accountGuid.Value, "Subject 1"));
            _email2Guid = _service.Create(CreateEmail(_accountGuid.Value, "Subject 2"));
            _email3Guid = _service.Create(CreateEmail(_accountGuid.Value, string.Empty));
        }

        private static Email CreateEmail(Guid accountGuid, string emailSubject)
        {
            //TODO check if the status actually works?
            return new Email()
            {
                RegardingObjectId = new EntityReference(Account.EntityLogicalName, accountGuid),
                Subject = emailSubject,
                ScheduledStart = DateTime.Now,
                StateCode = EmailState.Open
            };
        }

    }
}

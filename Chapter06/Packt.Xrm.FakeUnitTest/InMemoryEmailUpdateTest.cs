using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Packt.Xrm.Refactored.Plugin;
using Moq;
using Microsoft.Xrm.Sdk;
using Packt.Xrm.Entities;
using DLaB.Xrm.LocalCrm;
using DLaB.Xrm.Test;

namespace Packt.Xrm.FakeUnitTest
{
    [TestClass]
    public class InMemoryEmailUpdateTest
    {
        private const int NumberOfDaysDifference = 10;
        private IServiceProvider _serviceProvider;
        private Mock<ITracingService> _tracingMock;
        private IOrganizationService _organizationServiceFake;
        private Guid _email1Guid;
        private Guid _email2Guid;
        private Guid _email3Guid;

        [TestMethod]
        [TestCategory("InMemory")]
        public void Fake()
        {
            SetupMock();

            var plugin = new UpdateActivitiesRefactored();
            //Act
            plugin.Execute(_serviceProvider);


            //Assert
            Assert.AreEqual(_organizationServiceFake.GetEntity(new Id<Email>(_email1Guid)).StateCode, EmailState.Open);
            Assert.AreEqual(DateTime.Now.Date.AddDays(NumberOfDaysDifference), _organizationServiceFake.GetEntity(new Id<Email>(_email1Guid)).ScheduledStart, "Start date not updated correctly for emails with a subject");

            Assert.AreEqual(_organizationServiceFake.GetEntity(new Id<Email>(_email2Guid)).StateCode, EmailState.Open);
            Assert.AreEqual(DateTime.Now.Date.AddDays(NumberOfDaysDifference), _organizationServiceFake.GetEntity(new Id<Email>(_email2Guid)).ScheduledStart, "Start date not updated correctly for emails with a subject");

            Assert.AreEqual(EmailState.Canceled, _organizationServiceFake.GetEntity(new Id<Email>(_email3Guid)).StateCode, "Email without a subject was not cancelled");

            _tracingMock.Verify(trace => trace.Trace("{0} closed emails and {1} updated emails", 1, 2), Times.Exactly(1));
        }

        private void SetupMock()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            _tracingMock = new Mock<ITracingService>();

            //Organisation service
            _organizationServiceFake = new LocalCrmDatabaseOrganizationService(LocalCrmDatabaseInfo.Create<OrganisationServiceContext>());
            var accountGuid = _organizationServiceFake.Create(new Account() { Name = "Packt Test" });
            _email1Guid = _organizationServiceFake.Create(new Email() { Subject = "Mock 1", RegardingObjectId = new EntityReference(Account.EntityLogicalName, accountGuid) });
            _email2Guid = _organizationServiceFake.Create(new Email() { Subject = "Mock 2", RegardingObjectId = new EntityReference(Account.EntityLogicalName, accountGuid) });
            _email3Guid = _organizationServiceFake.Create(new Email() { Subject = string.Empty, RegardingObjectId = new EntityReference(Account.EntityLogicalName, accountGuid) });


            //Plugin Context
            var pluginContextMock = new Mock<IPluginExecutionContext>();
            //pluginContextMock.Setup(c => c.UserId).Returns(new Guid());

            //Target InputParameter for plugin context
            var parameterCollection = new ParameterCollection();
            parameterCollection.Add("Target", new Account() { Id = accountGuid });
            pluginContextMock.Setup(c => c.InputParameters).Returns(parameterCollection);

            //Factory
            var organisationServicefactoryMock = new Mock<IOrganizationServiceFactory>();
            organisationServicefactoryMock.Setup(f => f.CreateOrganizationService(It.IsAny<Guid>())).Returns(_organizationServiceFake);

            //Set up provider mothods
            serviceProviderMock.Setup(sp => sp.GetService(typeof(ITracingService))).Returns(_tracingMock.Object);
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IPluginExecutionContext))).Returns(pluginContextMock.Object);
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IOrganizationServiceFactory))).Returns(organisationServicefactoryMock.Object);

            _serviceProvider = serviceProviderMock.Object;
        }
    }
}

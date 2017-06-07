using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Packt.Xrm.Refactored.Plugin;
using Moq;
using Microsoft.Xrm.Sdk;
using Packt.Xrm.Entities;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using DLaB.Xrm.LocalCrm;

namespace Packt.Xrm.FakeUnitTest
{
    [TestClass]
    public class FakeUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {


            var serviceProviderMock = new Mock<IServiceProvider>();
            var traceServiceMock = new Mock<ITracingService>();

            //Plugin Context
            var pluginContextMock = new Mock<IPluginExecutionContext>();
            //pluginContextMock.Setup(c => c.UserId).Returns(new Guid());

            //Target InputParameter for plugin context
            var parameterCollection = new ParameterCollection();
            parameterCollection.Add("Target", new Account() { Id = Guid.NewGuid() });
            pluginContextMock.Setup(c => c.InputParameters).Returns(parameterCollection);

            //Organisation Service
            var organisationServiceMock = new Mock<IOrganizationService>();
            var entityCollection = new EntityCollection();
            entityCollection.Entities.Add(new Email() { Id = Guid.NewGuid(), Subject = "Mock 1" });
            entityCollection.Entities.Add(new Email() { Id = Guid.NewGuid(), Subject = "Mock 2" });
            entityCollection.Entities.Add(new Email() { Id = Guid.NewGuid()});
            organisationServiceMock.Setup(s => s.RetrieveMultiple(It.IsAny<QueryBase>())).Returns(entityCollection);


            //Factory
            var organisationServicefactoryMock = new Mock<IOrganizationServiceFactory>();
            organisationServicefactoryMock.Setup(f => f.CreateOrganizationService(It.IsAny<Guid>())).Returns(organisationServiceMock.Object);

            //Set up provider mothods
            serviceProviderMock.Setup(sp => sp.GetService(typeof(ITracingService))).Returns(traceServiceMock.Object);
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IPluginExecutionContext))).Returns(pluginContextMock.Object);
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IOrganizationServiceFactory))).Returns(organisationServicefactoryMock.Object);


            var plugin = new UpdateActivitiesRefactored();
            plugin.Execute(serviceProviderMock.Object);

            //var iserviceProvider = new IServiceProvider();
            //plugin.Execute();


            //Context validation
            organisationServiceMock.Verify(dal => dal.Execute(It.IsAny<SetStateRequest>()), Times.Exactly(1));
            organisationServiceMock.Verify(dal => dal.Update(It.IsAny<Entity>()), Times.Exactly(2));
            traceServiceMock.Verify(trace => trace.Trace("{0} closed emails and {1} updated emails", 1, 2), Times.Exactly(1));
        }
    }
}

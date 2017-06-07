using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Packt.Xrm.Refactored.BusinessLogic;
using Packt.Xrm.Refactored.DataAccessLayer;
using Moq;
using Packt.Xrm.Entities;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace Packt.Xrm.UnitTest
{
    [TestClass]
    public class EmailUpdateBusinessLogicTest
    {
        private Mock<IEmailDataAccessLayer> _emailDalMock;
        private Mock<ICustomTracingService> _tracing;

        [TestMethod]
        public void TestUpdateEmail()
        {
            //TODO does test goes last in the method name?
            //Arrange
            SetupMocks();

            var businessLogic = new UpdateEmailLogic(_emailDalMock.Object, _tracing.Object);

            //Act
            businessLogic.UpdateAccountsEmails(Guid.NewGuid());
            //Assert

            _emailDalMock.Verify(dal => dal.CloseEmailAsCancelled(It.IsAny<Email>()), Times.Exactly(1));
            _emailDalMock.Verify(dal => dal.UpdateEntity(It.IsAny<Entity>()), Times.Exactly(2));
            _tracing.Verify(trace => trace.Trace("{0} closed emails and {1} updated emails", 1, 2), Times.Exactly(1));
        }

        private void SetupMocks()
        {
            _emailDalMock = new Mock<IEmailDataAccessLayer>();
            _emailDalMock.Setup(dal => dal.GetEmails(It.IsAny<Guid>())).Returns(
                new List<Email>() { new Email() { Subject = "sample1" }, new Email { Subject = "sample2" }, new Email { Subject = string.Empty } }
                );

            _tracing = new Mock<ICustomTracingService>();
        }
    }
}

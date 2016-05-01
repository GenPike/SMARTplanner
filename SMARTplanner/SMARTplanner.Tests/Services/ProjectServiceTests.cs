using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Logic.Contracts;
using SMARTplanner.Logic.Exact;
using SMARTplanner.Tests.TestHelpers;

namespace SMARTplanner.Tests.Services
{
    [TestClass]
    public class ProjectServiceTests
    {
        [TestMethod]
        public void Can_GetProject_By_Id()
        {
            //Arrange
            #region Arrange

            var dbSet = new TestProjectDbSet();
            var mockContext = new Mock<ISmartPlannerContext>();
            var mockAccessService = new Mock<IAccessService>();

            var testProject = new Project { Id = 1 };
            dbSet.Add(testProject);
            mockContext.Setup(m => m.Projects).Returns(dbSet);
            mockAccessService.Setup(m => m.GetAccessByProject(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(new ProjectUserAccess());

            var projectService = new ProjectService(mockContext.Object, mockAccessService.Object);

            #endregion

            //Act
            var result = projectService.GetProject(testProject.Id, string.Empty);

            //Assert
            Assert.IsFalse(result.ErrorHandled);
            Assert.AreEqual(result.TargetObject.Id, testProject.Id);
        }

        [TestMethod]
        public void Can_GetProject_By_Name()
        {
            //Arrange
            #region Arrange

            var dbSet = new TestProjectDbSet();
            var mockContext = new Mock<ISmartPlannerContext>();
            var mockAccessService = new Mock<IAccessService>();

            var testProject = new Project { Name = "OutlookAddIn" };
            dbSet.Add(testProject);
            mockContext.Setup(m => m.Projects).Returns(dbSet);
            mockAccessService.Setup(m => m.GetAccessByProject(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(new ProjectUserAccess());

            var projectService = new ProjectService(mockContext.Object, mockAccessService.Object);

            #endregion

            //Act
            var result = projectService.GetProject(testProject.Name, string.Empty);

            //Assert
            Assert.IsFalse(result.ErrorHandled);
            Assert.AreEqual(result.TargetObject.Name, testProject.Name);
        }
    }
}

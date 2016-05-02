using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Entities.Helpers;
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

        [TestMethod]
        public void Can_Add_New_Project()
        {
            //Arrange
            #region Arrange

            var projectsDbSet = new TestProjectDbSet();
            var projUsersDbSet = new TestProjectUserAccessDbSet();
            var mockContext = new Mock<ISmartPlannerContext>();
            var mockAccessService = new Mock<IAccessService>();

            mockContext.Setup(m => m.Projects).Returns(projectsDbSet);
            var projectService = new ProjectService(mockContext.Object, mockAccessService.Object);

            #endregion

            //Act
            var result = projectService.AddProject(new Project
            {
                Id = 1,
                Name = "OutlookAddIn",
                CreatorId = "123",
                DateCreated = DateTime.Now,
                ProjectUsers = projUsersDbSet.Local
            });

            //Assert
            Assert.IsFalse(result.ErrorHandled);
            Assert.IsTrue(result.TargetObject);
            Assert.AreEqual(mockContext.Object.Projects.Count(), 1);
            Assert.AreEqual(projUsersDbSet.Count(), 1);
        }

        [TestMethod]
        public void Can_Get_All_Accessible_Projects()
        {
            //Arrange
            var projUsersDbSet = new TestProjectUserAccessDbSet();
            var mockContext = new Mock<ISmartPlannerContext>();

            string targetUserId = "123";
            projUsersDbSet.Add(new ProjectUserAccess { UserId = targetUserId, Project = new Project { Id = 1, CreatorId = targetUserId } });
            projUsersDbSet.Add(new ProjectUserAccess { UserId = targetUserId, Project = new Project { Id = 2 } });
            projUsersDbSet.Add(new ProjectUserAccess { UserId = "124", Project = new Project { Id = 3 } });
            mockContext.Setup(m => m.ProjectUserAccesses).Returns(projUsersDbSet);
            var accessService = new AccessService(mockContext.Object);
            var projectService = new ProjectService(mockContext.Object, accessService);

            //Act
            var result = projectService.GetProjectsPaged(targetUserId);

            //Assert
            Assert.IsFalse(result.ErrorHandled);
            Assert.AreEqual(result.TargetCollection.Count(), 2);
            Assert.AreEqual(projUsersDbSet.Count(), 3);
        }

        [TestMethod]
        public void Can_Get_Only_CreatedByMe_Projects()
        {
            //Arrange
            var projDbSet = new TestProjectDbSet();
            var mockContext = new Mock<ISmartPlannerContext>();

            string targetUserId = "123";
            projDbSet.Add(new Project { Id = 1, CreatorId = targetUserId });
            projDbSet.Add(new Project { Id = 2, CreatorId = "124" });
            projDbSet.Add(new Project { Id = 3, CreatorId = "125" });
            mockContext.Setup(m => m.Projects).Returns(projDbSet);
            var accessService = new AccessService(mockContext.Object);
            var projectService = new ProjectService(mockContext.Object, accessService);

            //Act
            var result = projectService.GetProjectsPaged(targetUserId, ProjectFilter.CreatedByMe);

            //Assert
            Assert.IsFalse(result.ErrorHandled);
            Assert.AreEqual(result.TargetCollection.Count(), 1);
            Assert.AreEqual(projDbSet.Count(), 3);
        }

        [TestMethod]
        public void Can_Get_Only_InvolvedIn_Projects()
        {
            //Arrange
            var projDbSet = new TestProjectDbSet();
            var projUsersDbSet = new TestProjectUserAccessDbSet();
            var mockContext = new Mock<ISmartPlannerContext>();

            string targetUserId = "123";
            var projects = new List<Project>
            {
                new Project { Id = 1, CreatorId = "124" },
                new Project { Id = 2, CreatorId = "124" },
                new Project { Id = 3, CreatorId = targetUserId },
                new Project { Id = 4, CreatorId = "125" }
            };
            projUsersDbSet.Add(new ProjectUserAccess { UserId = targetUserId, Project = projects[0] });
            projUsersDbSet.Add(new ProjectUserAccess { UserId = targetUserId, Project = projects[1] });
            projUsersDbSet.Add(new ProjectUserAccess { UserId = targetUserId, Project = projects[2] });

            projDbSet.AddRange(projects);
            mockContext.Setup(m => m.Projects).Returns(projDbSet);
            mockContext.Setup(m => m.ProjectUserAccesses).Returns(projUsersDbSet);
            var accessService = new AccessService(mockContext.Object);
            var projectService = new ProjectService(mockContext.Object, accessService);

            //Act
            var result = projectService.GetProjectsPaged(targetUserId, ProjectFilter.InvolvedIn);

            //Assert
            Assert.IsFalse(result.ErrorHandled);
            Assert.AreEqual(result.TargetCollection.Count(), 2);
            Assert.AreEqual(projDbSet.Count(), 4);
            Assert.AreEqual(projUsersDbSet.Count(), 3);
        }
    }
}

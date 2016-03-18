using System.Linq;
using SMARTplanner.Entities.Domain;

namespace SMARTplanner.Tests.TestHelpers
{
    class TestProjectDbSet : TestDbSet<Project>
    {
        public override Project Find(params object[] keyValues)
        {
            return this.SingleOrDefault(proj => proj.Id == (long)keyValues.Single());
        }
    }

    class TestProjectUserAccessDbSet : TestDbSet<ProjectUserAccess>
    {
        public override ProjectUserAccess Find(params object[] keyValues)
        {
            return this.SingleOrDefault(pu => pu.ProjectId == (long)keyValues[0] &&
                pu.UserId.Equals((string)keyValues[1]));
        }
    }

    class TestIssueDbSet : TestDbSet<Issue>
    {
        public override Issue Find(params object[] keyValues)
        {
            return this.SingleOrDefault(i => i.Id == (long)keyValues.Single());
        }
    }

    class TestWorkItemDbSet : TestDbSet<WorkItem>
    {
        public override WorkItem Find(params object[] keyValues)
        {
            return this.SingleOrDefault(w => w.Id == (long)keyValues.Single());
        }
    }

    class TestReportDbSet : TestDbSet<Report>
    {
        public override Report Find(params object[] keyValues)
        {
            return this.SingleOrDefault(r => r.Id == (long)keyValues.Single());
        }
    }
}

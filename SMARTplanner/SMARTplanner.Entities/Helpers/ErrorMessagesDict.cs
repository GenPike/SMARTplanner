using System;

namespace SMARTplanner.Entities.Helpers
{
    public static class ErrorMessagesDict
    {
        public static readonly string AccessDenied = "You don't have access to this resource";
        public static readonly string NotFoundResource = "No such resource founded";
        public static readonly string NullInstance = "Given instance is not initialized";
        public static readonly string InvalidPageSize = "There too much items per page";
        public static readonly string TooMuchProjectsCreated = "You cannot create project, there is limited number of projects per user to create";
        public static readonly string WrongEstimatedTimeFormat = "Wrong estimated time format";

    }
}

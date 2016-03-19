using System.Collections.Generic;
using SMARTplanner.Entities.Helpers;

namespace SMARTplanner.Logic.Contracts
{
    public interface ILogService<T> where T : ILogEntity
    {
        ServiceCollectionResult<T> GetItemHistory(long itemId, string userId);
        void LogAction(T log);
    }
}

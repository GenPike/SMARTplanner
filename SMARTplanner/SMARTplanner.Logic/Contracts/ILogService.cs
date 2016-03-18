using System.Collections.Generic;
using SMARTplanner.Entities.Helpers;

namespace SMARTplanner.Logic.Contracts
{
    public interface ILogService<T> where T : LogEntity
    {
        IEnumerable<T> GetItemHistory(long itemId);
        void LogAction(T log);
    }
}

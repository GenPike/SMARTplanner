using System.Collections.Generic;

namespace SMARTplanner.Entities.Helpers
{
    public abstract class ErrorResult
    {
        public bool ErrorHandled { get; set; }
        public string ErrorMessage { get; set; }
    }

    //Entities for returning any errors and results after service call
    public class ServiceSingleResult<T> : ErrorResult
    {
        public T TargetObject { get; set; }
    }

    public class ServiceCollectionResult<T> : ErrorResult
    {
        public IEnumerable<T> TargetCollection { get; set; }
    }
}

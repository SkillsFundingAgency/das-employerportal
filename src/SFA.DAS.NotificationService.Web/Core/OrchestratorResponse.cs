
using FluentValidation.Results;

namespace SFA.DAS.NotificationService.Web.Core
{
    public class OrchestratorResponse
    {
        public string Code { get; set; }

        public OrchestratorResponseMessage Message { get; set; }

        public object Parameters { get; set; }

        public ValidationResult ValidationResult { get; set; }
    }


    public class OrchestratorResponse<T> : OrchestratorResponse
    {
        public T ViewModel { get; set; }
    }
}
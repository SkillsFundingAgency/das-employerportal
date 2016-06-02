using FluentValidation.Results;

namespace SFA.DAS.NotificationService.Api.Core
{
    public abstract class OrchestratorBase
    {
        protected static OrchestratorResponse GetOrchestratorResponse(string code, ValidationResult validationResult = null, object parameters = null)
        {
            return new OrchestratorResponse
            {
                Code = code,
                ValidationResult = validationResult,
                Parameters = parameters
            };
        }

        protected static OrchestratorResponse GetOrchestratorResponse(string code, string message, UserMessageLevel level, object parameters = null)
        {
            var response = new OrchestratorResponse
            {
                Code = code,
                Message = new OrchestratorResponseMessage
                {
                    Text = message,
                    Level = level
                },
                Parameters = parameters
            };

            return response;
        }

        protected static OrchestratorResponse<T> GetOrchestratorResponse<T>(string code, T viewModel = default(T), ValidationResult validationResult = null, object parameters = null)
        {
            var response = new OrchestratorResponse<T>
            {
                Code = code,
                ViewModel = viewModel,
                ValidationResult = validationResult,
                Parameters = parameters
            };

            return response;
        }

        protected static OrchestratorResponse<T> GetOrchestratorResponse<T>(string code, T viewModel, string message, UserMessageLevel level, object parameters = null)
        {
            var response = new OrchestratorResponse<T>
            {
                Code = code,
                ViewModel = viewModel,
                Message = new OrchestratorResponseMessage
                {
                    Text = message,
                    Level = level
                },
                Parameters = parameters
            };

            return response;
        }
    }
}
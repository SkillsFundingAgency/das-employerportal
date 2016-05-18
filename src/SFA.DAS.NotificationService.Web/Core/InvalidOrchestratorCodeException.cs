using System;

namespace SFA.DAS.NotificationService.Web.Core
{
    public class InvalidOrchestratorCodeException : Exception
    {
        public string Code { get; set; }

        public InvalidOrchestratorCodeException(string code)
        {
            Code = code;
        }

        public override string Message => $"Orchestrator returned unrecognised code: {Code}";
    }
}
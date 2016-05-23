using System.Collections.Generic;

namespace SFA.DAS.NotificationService.Application.Interfaces
{
    public interface IEmailService
    {
        void Send(Dictionary<string, string> items);
    }
}
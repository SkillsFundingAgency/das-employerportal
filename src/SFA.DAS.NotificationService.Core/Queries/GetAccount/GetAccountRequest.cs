using MediatR;

namespace SFA.DAS.NotificationService.Application.Queries.GetAccount
{
    public class GetAccountRequest : IAsyncRequest<GetAccountResponse>
    {
        public int AccountId { get; set; }
    }
}

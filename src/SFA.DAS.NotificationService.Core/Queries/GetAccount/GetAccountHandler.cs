using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.NotificationService.Domain.Repositories;

namespace SFA.DAS.NotificationService.Application.Queries.GetAccount
{
    public class GetAccountHandler : IAsyncRequestHandler<GetAccountRequest, GetAccountResponse>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository; 
        }

        public async Task<GetAccountResponse> Handle(GetAccountRequest message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (message.AccountId < 1)
            {
                throw new ArgumentException("Request Account Id should be above zero");
            }

            var account = await _accountRepository.Get(message.AccountId);

            if (account == null)
            {
                return null;
            }

            return new GetAccountResponse
            {
                Account = account
            };
        }
    }
}

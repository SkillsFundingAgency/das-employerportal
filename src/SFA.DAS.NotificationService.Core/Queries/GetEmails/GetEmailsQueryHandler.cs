using System;
using MediatR;

namespace SFA.DAS.NotificationService.Application.Queries.GetEmails
{
    public class GetEmailsQueryHandler : IRequestHandler<GetEmailsQueryRequest, GetEmailsQueryResponse>
    {
        public GetEmailsQueryResponse Handle(GetEmailsQueryRequest message)
        {
            throw new NotImplementedException();
        }
    }
}
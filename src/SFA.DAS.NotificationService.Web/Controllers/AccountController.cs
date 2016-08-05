using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.NotificationService.Application.Queries.GetAccount;

namespace SFA.DAS.NotificationService.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<HttpResponseMessage> Get(int id)
        {
            if (id < 1)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("ID must be above zero.")
                };
            }

            var result = await _mediator.SendAsync(new GetAccountRequest { AccountId = id });

            if (result == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(result.Account))
            };
        }
    }
}
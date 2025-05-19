using Application.Wallets.Commands;
using Application.Wallets.Queries;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Application.Wallets.Dtos;
using MediatR;

namespace Api.Controllers
{
    //[Authorize]
    [RoutePrefix("api/wallets")]
    public class WalletController : ApiController
    {
        private readonly IMediator _mediator;

        public WalletController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create()
        {
            var wallet = await _mediator.Send(new CreateWalletCommand());
            return Ok(wallet);
        }

        [HttpPost, Route("{id:guid}/credit")]
        public async Task<IHttpActionResult> Credit(Guid id, [FromBody] CreditWalletRequest request)
        {
            var wallet = await _mediator.Send(new CreditWalletCommand(id, request));
            return Ok(wallet);
        }

        [HttpPost, Route("{id:guid}/debit")]
        public async Task<IHttpActionResult> Debit(Guid id, [FromBody] DebitWalletRequest request)
        {
            var wallet = await _mediator.Send(new DebitWalletCommand(id, request));
            return Ok(wallet);
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var wallet = await _mediator.Send(new GetWalletByIdQuery(id));
            return Ok(wallet);
        }

        [HttpGet, Route("all")]
        public async Task<IHttpActionResult> GetAll(int pageNumber, int pageSize)
        {
            var wallets = await _mediator.Send(new GetWalletsQuery(pageNumber, pageSize));
            return Ok(wallets);
        }

        [HttpGet, Route("transaction-id")]
        public IHttpActionResult GetNewTransactionId()
        {
            return Ok(Guid.NewGuid().ToString());
        }

        [HttpGet, Route("{id:guid}/row-version")]
        public async Task<IHttpActionResult> GetRowVersion(Guid id)
        {
            var wallet = await _mediator.Send(new GetWalletsRowVersionQuery(id));
            return Ok(wallet);
        }
    }
}
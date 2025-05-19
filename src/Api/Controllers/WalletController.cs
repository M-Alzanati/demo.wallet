using Application.Wallets.Commands;
using Application.Wallets.Queries;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using Application.DTOs;

namespace Api.Controllers
{
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
            var walletId = await _mediator.Send(new CreateWalletCommand());
            return Ok(walletId);
        }

        [HttpPost, Route("{id:guid}/credit")]
        public async Task<IHttpActionResult> Credit(Guid id, [FromBody] CreditWalletRequest request)
        {
            await _mediator.Send(new CreditWalletCommand(id, request));
            return Ok();
        }

        [HttpPost, Route("{id:guid}/debit")]
        public async Task<IHttpActionResult> Debit(Guid id, [FromBody] DebitWalletRequest request)
        {
            await _mediator.Send(new DebitWalletCommand(id, request));
            return Ok();
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
    }
}
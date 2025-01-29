using Application.Bill.Commends.Add;
using Application.Bill.Commends.Delete;
using Application.Bill.Commends.Update;
using Application.Bill.Queries.GetAll;
using Application.Bill.Queries.GetById;
using Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("GetAllBill")]
        public async Task<ActionResult<GetAllBillQueryResult>> GetAll()
        {
            return await this.HandleCommandResult(_mediator.Send(new GetAllBillQuery()));
        }
        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetBillByIdQueryResult>> GetById(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new GetBillByIdQuery(id)));
        }
        [HttpPost("AddBill")]
        public async Task<ActionResult<AddBillCommandResult>> Add([FromBody] AddBillCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }
        [HttpPut("UpdateBill")]
        public async Task<ActionResult<UpdateBillCommandResult>> Update([FromBody] UpdateBillCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }
        [HttpDelete("{id:long}")]
        public async Task<ActionResult<DeleteBillCommandResult>> Delete(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new DeleteBillCommand(id)));
        }

    }
}

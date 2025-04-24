using Application.Bill.Commends.Add;
using Application.Bill.Commends.Delete;
using Application.Bill.Commends.Update;
using Application.Bill.Queries.GetAll;
using Application.Bill.Queries.GetById;
using Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Bill
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Bill")]
    public class BillController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("GetAll")]
        public async Task<ActionResult<GetAllBillQueryResult>> GetAll([FromBody] GetAllBillQuery query)
        {
            return await this.HandleCommandResult(_mediator.Send(query));
        }

        [HttpGet("GetById/{id:long}")]
        public async Task<ActionResult<GetBillByIdQueryResult>> GetById(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new GetBillByIdQuery(id)));
        }

        [HttpPost("Add")]
        public async Task<ActionResult<AddBillCommandResult>> Add([FromBody] AddBillCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpPut("Update")]
        public async Task<ActionResult<UpdateBillCommandResult>> Update([FromBody] UpdateBillCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<ActionResult<DeleteBillCommandResult>> Delete(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new DeleteBillCommand(id)));
        }

    }
}

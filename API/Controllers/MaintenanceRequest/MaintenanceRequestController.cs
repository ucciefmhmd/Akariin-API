using Application.Bill.Commends.Delete;
using Application.MaintenanceRequest.Queries.GetAll;
using Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Contract
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "MaintenanceRequest")]
    public class MaintenanceRequestController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("GetAll")]
        public async Task<ActionResult<GetAllMaintenanceRequestQueryResult>> GetAll([FromBody] GetAllMaintenanceRequestQuery query)
        {
            return await this.HandleCommandResult(_mediator.Send(query));
        }

        //[HttpGet("GetById/{id:long}")]
        //public async Task<ActionResult<GetContractByIdQueryResult>> GetById(long id)
        //{
        //    return await this.HandleCommandResult(_mediator.Send(new GetContractByIdQuery(id)));
        //}

        //[HttpPost("Add")]
        //public async Task<ActionResult<AddContractCommandResult>> Add([FromForm] AddContractCommand command)
        //{
        //    return await this.HandleCommandResult(_mediator.Send(command));
        //}

        //[HttpPut("Update")]
        //public async Task<ActionResult<UpdateContractCommandResult>> Update([FromForm] UpdateContractCommand command)
        //{
        //    return await this.HandleCommandResult(_mediator.Send(command));
        //}

        //[HttpDelete("Delete/{id:long}")]
        //public async Task<ActionResult<DeleteBillCommandResult>> Delete(long id)
        //{
        //    return await this.HandleCommandResult(_mediator.Send(new DeleteBillCommand(id)));
        //}
    }
}

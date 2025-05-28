using Application.MaintenanceRequest.Commends.Add;
using Application.MaintenanceRequest.Commends.Update;
using Application.MaintenanceRequest.Queries.GetAll;
using Application.MaintenanceRequest.Queries.GetById;
using Application.Owner.Commends.Delete;
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

        [HttpGet("GetById/{id:long}")]
        public async Task<ActionResult<GetMaintenanceRequestByIdQueryResult>> GetById(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new GetMaintenanceRequestByIdQuery(id)));
        }

        [HttpPost("Add")]
        public async Task<ActionResult<AddMaintenanceRequestCommendResult>> Add([FromForm] AddMaintenanceRequestCommend command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpPut("Update")]
        public async Task<ActionResult<UpdateMaintenanceRequestCommendResult>> Update([FromForm] UpdateMaintenanceRequestCommend command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<ActionResult<DeleteMaintenanceRequestCommendResult>> Delete(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new DeleteMaintenanceRequestCommend(id)));
        }
    }
}

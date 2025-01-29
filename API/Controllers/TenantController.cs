using Application.RealEstate.Commends.AddRealEstate;
using Application.RealEstate.Commends.DeleteRealEstate;
using Application.RealEstate.Commends.UpdateRealEstate;
using Application.RealEstate.Queries.GetAllRealEstate;
using Application.RealEstate.Queries.GetByIdRealEstate;
using Application.Tenant.Commands.Add;
using Application.Tenant.Commands.Delete;
using Application.Tenant.Commands.Update;
using Application.Tenant.Queries.GetAll;
using Application.Tenant.Queries.GetById;
using Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("GetAllTenant")]
        public async Task<ActionResult<GetAllTenantQueryResult>> GetAll()
        {
            return await this.HandleCommandResult(_mediator.Send(new GetAllTenantQuery()));
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetTenantByIdQueryResult>> GetById(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new GetTenantByIdQuery(id)));
        }

        [HttpPost("AddTenant")]
        public async Task<ActionResult<AddTenantCommandResult>> Add([FromBody] AddTenantCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpPut("UpdateTenant")]
        public async Task<ActionResult<UpdateTenantCommandResult>> Update([FromBody] UpdateTenantCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<DeleteTenantCommandResult>> Delete(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new DeleteTenantCommand(id)));
        }
    }
}

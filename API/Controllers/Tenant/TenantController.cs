using Application.Tenant.Commands.Add;
using Application.Tenant.Commands.Delete;
using Application.Tenant.Commands.Update;
using Application.Tenant.Queries.GetAll;
using Application.Tenant.Queries.GetById;
using Asp.Versioning;
using Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Tenant
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Tenant")]
    [ApiVersion("1.0")]
    public class TenantController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("GetAllTenant")]
        public async Task<ActionResult<GetAllTenantQueryResult>> GetAll([FromBody] GetAllTenantQuery query)
        {
            return await this.HandleCommandResult(_mediator.Send(query));
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

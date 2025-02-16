using Application.Owner.Commends.Add;
using Application.Owner.Commends.Delete;
using Application.Owner.Commends.Update;
using Application.Owner.Queries.GetAll;
using Application.Owner.Queries.GetById;
using Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Owner
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Owner")]
    public class OwnerController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("GetAll")]
        public async Task<ActionResult<GetAllOwnersQueryResult>> GetAll([FromBody] GetAllOwnersQuery query)
        {
            return await this.HandleCommandResult(_mediator.Send(query));
        }

        [HttpGet("GetById/{id:long}")]
        public async Task<ActionResult<GetOwnerByIdQueryResult>> GetById(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new GetOwnerByIdQuery(id)));
        }

        [HttpPost("Add")]
        public async Task<ActionResult<AddOwnerCommandResult>> Add([FromBody] AddOwnerCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpPut("Update")]
        public async Task<ActionResult<UpdateOwnerCommandResult>> Update([FromBody] UpdateOwnerCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<ActionResult<DeleteOwnerCommandResult>> Delete(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new DeleteOwnerCommand(id)));
        }
    }
}

using Application.Owner.Commends.Add;
using Application.Owner.Commends.Delete;
using Application.Owner.Commends.Update;
using Application.Owner.Queries.GetAll;
using Application.Owner.Queries.GetById;
using Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("GetAllOwners")]
        public async Task<ActionResult<GetAllOwnersQueryResult>> GetAll()
        {
            return await this.HandleCommandResult(_mediator.Send(new GetAllOwnersQuery()));
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetOwnerByIdQueryResult>> GetById(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new GetOwnerByIdQuery(id)));
        }

        [HttpPost("AddOwner")]
        public async Task<ActionResult<AddOwnerCommandResult>> Add([FromBody] AddOwnerCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpPut("UpdateOwner")]
        public async Task<ActionResult<UpdateOwnerCommandResult>> Update([FromBody] UpdateOwnerCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<DeleteOwnerCommandResult>> Delete(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new DeleteOwnerCommand(id)));
        }
    }
}

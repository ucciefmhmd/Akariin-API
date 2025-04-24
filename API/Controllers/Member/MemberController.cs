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
    [ApiExplorerSettings(GroupName = "Member")]
    public class MemberController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("GetAll")]
        public async Task<ActionResult<GetAllMembersQueryResult>> GetAll([FromBody] GetAllMembersQuery query)
        {
            return await this.HandleCommandResult(_mediator.Send(query));
        }

        [HttpGet("GetById/{id:long}")]
        public async Task<ActionResult<GetMemberByIdQueryResult>> GetById(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new GetMemberByIdQuery(id)));
        }

        [HttpPost("Add")]
        public async Task<ActionResult<AddMemberCommandResult>> Add([FromBody] AddMemberCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpPut("Update")]
        public async Task<ActionResult<UpdateMemberCommandResult>> Update([FromBody] UpdateMemberCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<ActionResult<DeleteMemberCommandResult>> Delete(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new DeleteMemberCommand(id)));
        }


    }
}

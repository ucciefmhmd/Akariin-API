using Application.Contract.Commends.Add;
using Application.Contract.Commends.Delete;
using Application.Contract.Commends.Update;
using Application.Contract.Queries.GetAll;
using Application.Contract.Queries.GetById;
using Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Contract
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Contract")]
    public class ContractController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("GetAll")]
        public async Task<ActionResult<GetAllContractQueryResult>> GetAll([FromBody] GetAllContractQuery query)
        {
            return await this.HandleCommandResult(_mediator.Send(query));
        }

        [HttpGet("GetById/{id:long}")]
        public async Task<ActionResult<GetContractByIdQueryResult>> GetById(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new GetContractByIdQuery(id)));
        }

        [HttpPost("Add")]
        public async Task<ActionResult<AddContractCommandResult>> Add([FromForm] AddContractCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpPut("Update")]
        public async Task<ActionResult<UpdateContractCommandResult>> Update([FromForm] UpdateContractCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<ActionResult<DeleteContractCommandResult>> Delete(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new DeleteContractCommand(id)));
        }
    }
}

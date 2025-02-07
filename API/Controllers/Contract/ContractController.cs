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
        [HttpPost("GetAllContract")]
        public async Task<ActionResult<GetAllContractQueryResult>> GetAll([FromBody] GetAllContractQuery query)
        {
            return await this.HandleCommandResult(_mediator.Send(query));
        }
        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetContractByIdQueryResult>> GetById(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new GetContractByIdQuery(id)));
        }
        [HttpPost("AddContract")]
        public async Task<ActionResult<AddContractCommandResult>> Add([FromBody] AddContractCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }
        [HttpPut("UpdateContract")]
        public async Task<ActionResult<UpdateContractCommandResult>> Update([FromBody] UpdateContractCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }
        [HttpDelete("{id:long}")]
        public async Task<ActionResult<DeleteContractCommandResult>> Delete(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new DeleteContractCommand(id)));
        }
    }
}

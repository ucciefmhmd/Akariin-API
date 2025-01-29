using Application.RealEstateUnit.Commends.Add;
using Application.RealEstateUnit.Commends.Delete;
using Application.RealEstateUnit.Commends.Update;
using Application.RealEstateUnit.Queries.GetAll;
using Application.RealEstateUnit.Queries.GetById;
using Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealEstateUnitController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("GetAllRealEstateUnit")]
        public async Task<ActionResult<GetAllRealEstateUnitQueryResult>> GetAll()
        {
            return await this.HandleCommandResult(_mediator.Send(new GetAllRealEstateUnitQuery()));
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetRealEstateUnitByIdQueryResult>> GetById(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new GetRealEstateUnitByIdQuery(id)));
        }

        [HttpPost("AddRealEstateUnit")]
        public async Task<ActionResult<AddRealEstateUnitCommendResult>> Add([FromBody] AddRealEstateUnitCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpPut("UpdateRealEstateUnit")]
        public async Task<ActionResult<UpdateRealEstateUnitCommendResult>> Update([FromBody] UpdateRealEstateUnitCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<DeleteRealEstateUnitCommandResult>> Delete(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new DeleteRealEstateUnitCommand(id)));
        }
    }
}


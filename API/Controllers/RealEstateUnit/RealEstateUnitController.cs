using Application.RealEstateUnit.Commends.Add;
using Application.RealEstateUnit.Commends.Delete;
using Application.RealEstateUnit.Commends.Update;
using Application.RealEstateUnit.Queries.GetAll;
using Application.RealEstateUnit.Queries.GetById;
using Common;
using Domain.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.RealEstateUnit
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "RealEstateUnit")]
    public class RealEstateUnitController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("GetAll")]
        public async Task<ActionResult<GetAllRealEstateUnitQueryResult>> GetAll([FromBody] GetAllRealEstateUnitQuery query)
        {
            return await this.HandleCommandResult(_mediator.Send(query));
        }

        [HttpGet("GetById/{id:long}")]
        public async Task<ActionResult<GetRealEstateUnitByIdQueryResult>> GetById(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new GetRealEstateUnitByIdQuery(id)));
        }

        [HttpPost("Add")]
        [Authorize(Roles = $"{Roles.ADMIN},{Roles.SUB_ADMIN},{Roles.USER}")]
        public async Task<ActionResult<AddRealEstateUnitCommendResult>> Add([FromForm] AddRealEstateUnitCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command)); 
        }

        [HttpPut("Update")]
        public async Task<ActionResult<UpdateRealEstateUnitCommendResult>> Update([FromForm] UpdateRealEstateUnitCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<ActionResult<DeleteRealEstateUnitCommandResult>> Delete(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new DeleteRealEstateUnitCommand(id)));
        }
    }
}


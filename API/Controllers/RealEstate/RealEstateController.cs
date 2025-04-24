using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.RealEstate.Commends.AddRealEstate;
using Application.RealEstate.Commends.DeleteRealEstate;
using Application.RealEstate.Commends.UpdateRealEstate;
using Application.RealEstate.Queries.GetAllRealEstate;
using Application.RealEstate.Queries.GetByIdRealEstate;
using Common;
using Microsoft.AspNetCore.Authorization;
using Domain.Common.Constants;

namespace API.Controllers.RealEstate
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "RealEstate")]
    public class RealEstateController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("GetAllRealEstate")]
        public async Task<ActionResult<GetAllRealEstateQueryResult>> GetAll([FromBody] GetAllRealEstateQuery query)
        {
            return await this.HandleCommandResult(_mediator.Send(query));
        }

        [HttpGet("GetById/{id:long}")]
        public async Task<ActionResult<GetRealEstateByIdQueryResult>> GetById(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new GetRealEstateByIdQuery(id)));
        }

        [HttpPost("AddRealEstate")]
        [Authorize(Roles = $"{Roles.ADMIN},{Roles.SUB_ADMIN},{Roles.USER}")]
        public async Task<ActionResult<AddRealEstateCommandResult>> Add([FromForm] AddRealEstateCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpPut("UpdateRealEstate")]
        public async Task<ActionResult<UpdateRealEstateCommandResult>> Update([FromForm] UpdateRealEstateCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<ActionResult<DeleteRealEstateCommandResult>> Delete(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new DeleteRealEstateCommand(id)));
        }

    }
}

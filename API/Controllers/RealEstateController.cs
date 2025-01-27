using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.RealEstate.Commends.AddRealEstate;
using Application.RealEstate.Commends.DeleteRealEstate;
using Application.RealEstate.Commends.UpdateRealEstate;
using Application.RealEstate.Queries.GetAllRealEstate;
using Application.RealEstate.Queries.GetByIdRealEstate;
using Common;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealEstateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RealEstateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllRealEstate")]
        public async Task<ActionResult<GetAllRealEstateQueryResult>> GetAll()
        {
            return await this.HandleCommandResult(_mediator.Send(new GetAllRealEstateQuery()));
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetRealEstateByIdQueryResult>> GetById(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new GetRealEstateByIdQuery(id)));
        }

        [HttpPost("AddRealEstate")]
        public async Task<ActionResult<AddRealEstateCommandResult>> Add([FromBody] AddRealEstateCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpPut("UpdateRealEstate")]
        public async Task<ActionResult<UpdateRealEstateCommandResult>> Update([FromBody] UpdateRealEstateCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<DeleteRealEstateCommandResult>> Delete(long id)
        {
            return await this.HandleCommandResult(_mediator.Send(new DeleteRealEstateCommand(id)));
        }
    }
}

using Application.Summary.Queries.FetchSystemSummary;
using Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Summary
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Summary")]
    public class SummaryController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("FetchSystemSummary")]
        public async Task<ActionResult<FetchSystemSummaryQueryResult>> FetchSystemSummary()
        {
            return await this.HandleCommandResult(_mediator.Send(new FetchSystemSummaryQuery()));
        }
    }
}
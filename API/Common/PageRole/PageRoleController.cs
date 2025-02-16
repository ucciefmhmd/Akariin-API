using Application.Common.RoleSystem.Commands.AddRolesInPage;
using Application.Common.RoleSystem.Commands.DeleteRolesFromPage;
using Application.Common.RoleSystem.Commands.UpdatePageRoles;
using Application.Common.RoleSystem.Queries.GetPages;
using Application.Common.RoleSystem.Queries.GetPagesAndRoles;
using Application.Common.RoleSystem.Queries.GetPagesAndRolesForCheck;
using Application.Common.RoleSystem.Queries.GetRolesInPage;
using Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Common.PageRole
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Common")]
    [ApiController]
    public class PageRoleController(IMediator _mediator) : ControllerBase
    {

        [HttpPost("AddRolesInPage")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AddRolesInPageCommandResult>> AddRolesInPage([FromBody] AddRolesInPageCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpPost("DeleteRolesFromPage")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DeleteRolesFromPageCommandResult>> DeleteRolesFromPage([FromBody] DeleteRolesFromPageCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }


        [HttpPost("ResetRolesFromUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DeleteRolesFromPageCommandResult>> ResetRolesFromUser([FromBody] string userId)
        {
            var command = new DeleteRolesFromPageCommand { UserId = userId, PageId = null, Roles = { } };
            return await this.HandleCommandResult(_mediator.Send(command));
        }


        [HttpPost("UpdatePageRoles")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UpdatePageRolesCommandResult>> UpdatePageRoles([FromBody] UpdatePageRolesCommand command)
        {
            return await this.HandleCommandResult(_mediator.Send(command));
        }

        [HttpPost("GetRolesInPage")]
        [Authorize(Roles = "Admin,SubAdmin")]
        public async Task<ActionResult<GetRolesInPageQueryResult>> GetRolesInPage([FromBody] GetRolesInPageQuery query)
        {
            return await this.HandleCommandResult(_mediator.Send(query));
        }

        [HttpPost("GetPagesAndRoles")]
        [Authorize(Roles = "Admin,SubAdmin")]
        public async Task<ActionResult<GetPagesAndRolesQueryResult>> GetPagesAndRoles([FromBody] GetPagesAndRolesQuery query)
        {
            return await this.HandleCommandResult(_mediator.Send(query));
        }


        [HttpPost("GetPages")]
        [Authorize(Roles = "Admin,SubAdmin")]
        public async Task<ActionResult<GetPagesQueryResult>> GetPages([FromBody] GetPagesQuery query)
        {
            return await this.HandleCommandResult(_mediator.Send(query));
        }


        [HttpPost("GetPagesAndRolesForCheck")]
        [Authorize(Roles = "Admin,SubAdmin")]
        public async Task<ActionResult<GetPagesAndRolesForCheckQueryResult>> GetPages([FromBody] GetPagesAndRolesForCheckQuery query)
        {
            return await this.HandleCommandResult(_mediator.Send(query));
        }

    }
}

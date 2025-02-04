﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Attachment.Commands.DeleteAttachment;
using Asp.Versioning;

namespace Common.Controllers.Attachment
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Common")]
    [ApiController]
    [Authorize]
    public class AttachmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AttachmentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("DeleteFile")]
        public async Task<ActionResult<DeleteAttachmentCommandResult>> DeleteFile([FromForm] DeleteAttachmentCommand command)
        {

            return await this.HandleCommandResult(_mediator.Send(command));

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Application.Chat.Queries.GetChatHistory;
using Application.Chat.Commands.SendMessage;
using System.IdentityModel.Tokens.Jwt;

namespace API.Hubs
{

    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        private static readonly Dictionary<string, string> _connectedUsers = new(); // Keeps track of connected users

        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        // When a user connects, send them the chat history and store their connection
        public override async Task OnConnectedAsync()
        {
            var receiverId = Context.GetHttpContext().Request.Query["receiverId"].ToString();
            var userId = Context.User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            var claims = Context.User.Claims;

            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(receiverId))
            {
                foreach (var claim in claims)
                {
                    await Clients.Caller.SendAsync("Error", claim.Type + "||" + claim.Value);
                }
                await Clients.Caller.SendAsync("Error", $"User ID or receiver ID not provided.");
                return;
            }

            // Store the connected user's ID and connection ID
            if (!_connectedUsers.ContainsKey(userId))
            {
                _connectedUsers.Add(userId, Context.ConnectionId);
            }
            else
            {
                _connectedUsers[userId] = Context.ConnectionId; // Update the connection ID if the user reconnects
            }

            // Get the chat history from the mediator
            var chatHistoryQuery = new GetChatHistoryQuery { ReceiverId = receiverId };
            var chatHistoryResult = await _mediator.Send(chatHistoryQuery);

            if (chatHistoryResult.IsSuccess)
            {
                await Clients.Caller.SendAsync("ReceiveChatHistory", chatHistoryResult);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", chatHistoryResult);
            }

            await base.OnConnectedAsync();
        }

        // When a user disconnects, remove them from the connected users list
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;

            if (_connectedUsers.ContainsKey(userId))
            {
                _connectedUsers.Remove(userId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Method to send a private message via SignalR
        public async Task SendMessage(string receiverId, string message)
        {
            var userId = Context.User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            var senderId = userId;

            //if (!_connectedUsers.ContainsKey(receiverId))
            //{
            //    await Clients.Caller.SendAsync("Error", "The recipient is not connected.");
            //    return;
            //}

            var sendMessageCommand = new SendMessageCommand
            {
                ReceiverId = receiverId,
                SenderId = senderId,
                Message = message
            };

            var result = await _mediator.Send(sendMessageCommand);

            if (result.IsSuccess)
            {
                var receiverConnectionId = _connectedUsers[receiverId];

                if(!string.IsNullOrEmpty(receiverConnectionId))
                {
                    // Send the message to the recipient only
                    await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", result);
                }

                // Optionally, send the message back to the sender for UI confirmation
                await Clients.Caller.SendAsync("ReceiveMessage",result);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", result);
            }
        }
    }


}

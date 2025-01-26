using Application.Common.User.Commands.UpdateUserContact;
using Application.Common.User.Commands.UpdateUserSocials;
using Application.Services.UserService;
using Application.Utilities.Models;
using Domain.Common;
using Domain.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.User.Queries.GetUserContact
{

    public record GetUserContactQueryResult : BaseCommandResult
    {
        public UserContactDataVM User { get; set; }
    }
    public record GetUserContactQuery : IRequest<GetUserContactQueryResult>
    {

    }

    public class GetUserContactHandler : IRequestHandler<GetUserContactQuery, GetUserContactQueryResult>
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public GetUserContactHandler(IUserService userService,ApplicationDbContext context)
        {
            this._userService = userService;
            this._context = context;
        }
        public async Task<GetUserContactQueryResult> Handle(GetUserContactQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var contextUser = await _userService.GetCurrentUserAsync();
                var user = await _context.Users
                    .Include(u => u.District)
                    .ThenInclude(u => u.City)
                    .ThenInclude(u => u.Country)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(u =>  u.Id == contextUser.Id);
                if (user == null)
                {
                    return new GetUserContactQueryResult
                    {
                        ErrorCode = ErrorCode.AccessDenied,
                        Errors = { "Please Login First" },
                        IsSuccess = false
                    };
                }
                
               

                return new GetUserContactQueryResult
                {
                    IsSuccess = true,
                    User = new UserContactDataVM
                    {
                        City = user.District != null ? user.District.City.Name:"",
                        CityId = user.District != null ? user.District.CityId : null,
                        Country = user.District != null ? user.District.City.Country?.Name : "",
                        CountryId = user.District != null ? user.District.City.CountryId : null,
                        Id = user.Id,
                        District = user.District != null ? user.District.Name : "",
                        DistrictId = user.DistrictId,
                        ZIP = user.ZIP,
                        Fax = user.Fax,
                        AddressLine1 = user.AddressLine1,
                        AddressLine2 = user.AddressLine2
                    }
                };
            }
            catch (Exception ex)
            {
                return new GetUserContactQueryResult
                {
                    ErrorCode = ErrorCode.Error,
                    Errors = { ex.Message },
                    IsSuccess = false
                };
            }

        }
    }
}

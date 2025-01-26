using Application.Common.User.Commands.UpdateUserContact;
using Application.Common.User.Commands.UpdateUserProfile;
using Application.Services.File;
using Application.Services.UserService;
using Application.Utilities.Models;
using Domain.Common;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.User.Queries.GetUserProfile
{
    public record GetUserProfileQueryResult : BaseCommandResult
    {
        public UserProfileDataVM User { get; set; }
    }
    public record GetUserProfileQuery : IRequest<GetUserProfileQueryResult>
    {

    }

    public class GetUserContactHandler : IRequestHandler<GetUserProfileQuery, GetUserProfileQueryResult>
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        private readonly AttachmentService _attachmentService;

        public GetUserContactHandler(IUserService userService, LocalFiletService localFiletService,ApplicationDbContext context)
        {
            this._userService = userService;
            this._context = context;
            _attachmentService = new AttachmentService(localFiletService);
        }
        public async Task<GetUserProfileQueryResult> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user == null)
                {
                    return new GetUserProfileQueryResult
                    {
                        ErrorCode = ErrorCode.AccessDenied,
                        Errors = { "Please Login First" },
                        IsSuccess = false
                    };
                }
                var url = "";
                var profile = await _attachmentService.GetFilesUrlAsync(Path.Combine("profiles", user.Id));
                if (profile.IsSuccess)
                {
                    url = profile.Urls.Count > 0 ? profile.Urls[0] : "";
                }
                
                
                
                var profileUrls = new List<string>();
                var profileImages = await _attachmentService.GetFilesUrlAsync(Path.Combine("profiles", "profileImages", user.Id));
                if (profileImages.IsSuccess)
                {
                    profileUrls = profileImages.Urls;
                }


                var cities = await _context.ApplicationUserCities.Include(c => c.City)
                    .Where(c => c.ApplicationUserId == user.Id)
                    .Select(c => new cityUserVM
                    {
                        CityId = c.CityId,
                        CityName = c.City.Name
                    })
                    .ToListAsync();
                
                var services = await _context.ApplicationUserServices.Include(c => c.Service)
                    .Where(c => c.ApplicationUserId == user.Id)
                    .Select(c => new serviceUserVM
                    {
                        ServiceId = c.ServiceId,
                        ServiceName = c.Service.ServiceName
                    })
                    .ToListAsync();
               
                return new GetUserProfileQueryResult
                {
                    IsSuccess = true,
                    User = new UserProfileDataVM
                    {
                        AboutMe = user.AboutMe,
                        FirstName = user.FirstName,
                        Id = user.Id,
                        LastName = user.LastName,
                        MyFavStyle = user.MyFavStyle,
                        MyNextHouseProject = user.MyNextHouseProject,
                        Image = url,
                        LicenseNumber = user.LicenseNumber,
                        BusinessDescription = user.BusinessDescription,
                        CertificationAndAwards = user.CertificationAndAwards,
                        Affiliations = user.Affiliations,
                        JobCostFrom = user.JobCostFrom??0,
                        JobCostTo = user.JobCostTo??0,
                        CostDetails = user.CostDetails,
                        ProfileImages = profileUrls,
                        ApplicationUserCities = cities,
                        ApplicationUserServices = services,
                    }
                };
            }
            catch (Exception ex)
            {
                return new GetUserProfileQueryResult
                {
                    ErrorCode = ErrorCode.Error,
                    Errors = { ex.Message },
                    IsSuccess = false
                };
            }

        }
    }
}

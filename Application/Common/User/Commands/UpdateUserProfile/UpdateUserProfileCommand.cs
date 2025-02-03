using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Utilities.Models;
using Domain.Common;
using Domain.Common.Attributes;
using Infrastructure;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Application.Services.File;
namespace Application.Common.User.Commands.UpdateUserProfile
{
    public record UpdateUserProfileCommandResult : BaseCommandResult
    {
        public UserProfileDataVM User { get; set; }
    }
    public record UpdateUserProfileCommand : IRequest<UpdateUserProfileCommandResult>
    {
        [Required]
        public string Id { get; set; }
        [Trim]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AboutMe { get; set; }
        public string? MyFavStyle { get; set; }
        public string? MyNextHouseProject { get; set; }
        public IFormFile? Image { get; set; }
        public string? LicenseNumber { get; set; }
        public string? BusinessDescription { get; set; }
        public string? CertificationAndAwards { get; set; }
        public string? Affiliations { get; set; }
        public List<Guid> ServiceIds { get; set; } = new List<Guid>();
        public List<Guid> CityIds { get; set; } = new List<Guid>();
        [Precision(18, 4)]
        public decimal? JobCostFrom { get; set; }
        [Precision(18, 4)]
        public decimal? JobCostTo { get; set; }
        public string? CostDetails { get; set; }
        public IFormFileCollection? ProfileImages { get; set; }
    }
    public class UserProfileDataVM
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AboutMe { get; set; }
        public string? MyFavStyle { get; set; }
        public string? MyNextHouseProject { get; set; }
        public string Image { get; set; }


        public string? LicenseNumber { get; set; }
        public string? BusinessDescription { get; set; }
        public string? CertificationAndAwards { get; set; }
        public string? Affiliations { get; set; }

        [Precision(18, 4)]
        public decimal? JobCostFrom { get; set; }
        [Precision(18, 4)]
        public decimal? JobCostTo { get; set; }
        public string? CostDetails { get; set; }
        public List<string> ProfileImages { get; set; } = new List<string>();
        public List<serviceUserVM> ApplicationUserServices { get; set; } = new List<serviceUserVM>();
        public List<cityUserVM> ApplicationUserCities { get; set; } = new List<cityUserVM>();


    }
    public record serviceUserVM
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
    }

    public record cityUserVM
    {
        public Guid CityId { get; set; }
        public string CityName { get; set; }
    }
    //public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UpdateUserProfileCommandResult>
    //{
    //    private readonly ApplicationDbContext _context;
    //    private readonly AttachmentService _attachmentService;

    //    public UpdateUserProfileCommandHandler(ApplicationDbContext context, LocalFiletService localFiletService)
    //    {
    //        _context = context;
    //        _attachmentService = new AttachmentService(localFiletService);
    //    }
    //    public async Task<UpdateUserProfileCommandResult> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    //    {
    //        using (var trans = await _context.Database.BeginTransactionAsync(cancellationToken))
    //        {
    //            try
    //            {
    //                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
    //                if (user == null)
    //                {
    //                    await trans.RollbackAsync(cancellationToken);
    //                    return new UpdateUserProfileCommandResult
    //                    {
    //                        ErrorCode = ErrorCode.UserNotFound,
    //                        Errors = { "User Not Found" },
    //                        IsSuccess = false
    //                    };
    //                }
    //                user.FirstName = request.FirstName;
    //                user.LastName = request.LastName;
    //                user.AboutMe = request.AboutMe;
    //                user.BusinessDescription = request.BusinessDescription;
    //                user.CertificationAndAwards = request.CertificationAndAwards;


    //                _context.Users.Update(user);
    //                await _context.SaveChangesAsync();


    //                var oldServices = await _context.ApplicationUserServices.Where(s => s.ApplicationUserId == user.Id).ToListAsync();
    //                _context.ApplicationUserServices.RemoveRange(oldServices);
    //                await _context.SaveChangesAsync();
    //                foreach (var serviceId in request.ServiceIds)
    //                {
    //                    if (!await _context.Services.AnyAsync(s => s.Id == serviceId))
    //                    {
    //                        await trans.RollbackAsync(cancellationToken);
    //                        return new UpdateUserProfileCommandResult
    //                        {
    //                            IsSuccess = false,
    //                            ErrorCode = ErrorCode.NotFound,
    //                            Errors = { $"service with Id {serviceId} not found" }
    //                        };
    //                    }
    //                    var service = new ApplicationUserService
    //                    {
    //                        ApplicationUserId = user.Id,
    //                        ServiceId = serviceId,
    //                    };
    //                    _context.ApplicationUserServices.Add(service);
    //                }
    //                await _context.SaveChangesAsync();


    //                var oldCities = await _context.ApplicationUserCities.Where(s => s.ApplicationUserId == user.Id).ToListAsync();
    //                _context.ApplicationUserCities.RemoveRange(oldCities);
    //                await _context.SaveChangesAsync();
    //                foreach (var cityId in request.CityIds)
    //                {
    //                    if (!await _context.Cities.AnyAsync(s => s.Id == cityId))
    //                    {
    //                        await trans.RollbackAsync(cancellationToken);
    //                        return new UpdateUserProfileCommandResult
    //                        {
    //                            IsSuccess = false,
    //                            ErrorCode = ErrorCode.NotFound,
    //                            Errors = { $"City with Id {cityId} not found" }
    //                        };
    //                    }
    //                    var city = new ApplicationUserCity
    //                    {
    //                        ApplicationUserId = user.Id,
    //                        CityId = cityId,
    //                    };
    //                    _context.ApplicationUserCities.Add(city);
    //                }
    //                await _context.SaveChangesAsync();
    //                string url = "";
    //                if (request.Image != null)
    //                {
    //                    await _attachmentService.DeleteFilesAsync(Path.Combine("profiles", user.Id));
    //                    var Image = new FormFileCollection
    //                    {
    //                        request.Image,
    //                    };
    //                    await _attachmentService.UploadFilesAsync(Path.Combine("profiles", user.Id), Image);
    //                    var profile = await _attachmentService.GetFilesUrlAsync(Path.Combine("profiles", request.Id));
    //                    if (profile.IsSuccess)
    //                    {
    //                        url = profile.Urls.Count > 0 ? profile.Urls[0] : "";
    //                    }
    //                }
    //                var profileUrls = new List<string>();
    //                if (request.ProfileImages != null)
    //                {
    //                    await _attachmentService.UploadFilesAsync(Path.Combine("profiles", "profileImages", user.Id), request.ProfileImages);
    //                    var profile = await _attachmentService.GetFilesUrlAsync(Path.Combine("profiles", "profileImages", request.Id));
    //                    if (profile.IsSuccess)
    //                    {
    //                        profileUrls = profile.Urls;
    //                    }
    //                }
    //                await trans.CommitAsync(cancellationToken);
    //                return new UpdateUserProfileCommandResult
    //                {
    //                    IsSuccess = true,
    //                    User = new UserProfileDataVM
    //                    {
    //                        AboutMe = user.AboutMe,
    //                        FirstName = user.FirstName,
    //                        Id = user.Id,
    //                        LastName = user.LastName,
    //                        MyFavStyle = user.MyFavStyle,
    //                        MyNextHouseProject = user.MyNextHouseProject,
    //                        Image = url,
    //                        LicenseNumber = user.LicenseNumber,
    //                        BusinessDescription = user.BusinessDescription,
    //                        CertificationAndAwards = user.CertificationAndAwards,
    //                        Affiliations = user.Affiliations,
    //                        JobCostFrom = user.JobCostFrom ?? 0,
    //                        JobCostTo = user.JobCostTo ?? 0,
    //                        CostDetails = user.CostDetails,
    //                        ProfileImages = profileUrls
    //                    }
    //                };
    //            }
    //            catch (Exception ex)
    //            {
    //                await trans.RollbackAsync(cancellationToken);
    //                return new UpdateUserProfileCommandResult
    //                {
    //                    ErrorCode = ErrorCode.Error,
    //                    Errors = { ex.Message },
    //                    IsSuccess = false
    //                };
    //            }
    //        }


    //    }
    //}


}

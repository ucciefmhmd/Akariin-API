using Application.Services.File;
using Application.Utilities.Models;
using Domain.Common;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.User.Commands.UpdateUserBillingInfo
{

    public record UpdateUserBillingInfoCommandResult : BaseCommandResult
    {
        public UserBillingInfoVM User { get; set; }
    }
    public record UpdateUserBillingInfoCommand : IRequest<UpdateUserBillingInfoCommandResult>
    {
        [Required]
        public string Id { get; set; }
        public string? BillingFirstName { get; set; }
        public string? BillingFatherName { get; set; }
        public string? BillingFamilyName { get; set; }
        public string? BillingFullNameInEnglish { get; set; }
        public string? BillingMainPhoneNumber { get; set; }
        public string? BillingAlternativePhoneNumber { get; set; }
    }
    public class UserBillingInfoVM
    {
        public string Id { get; set; }
        public string? BillingFirstName { get; set; }
        public string? BillingFatherName { get; set; }
        public string? BillingFamilyName { get; set; }
        public string? BillingFullNameInEnglish { get; set; }
        public string? BillingMainPhoneNumber { get; set; }
        public string? BillingAlternativePhoneNumber { get; set; }
    }
    public class UpdateUserBillingInfoCommandHandler : IRequestHandler<UpdateUserBillingInfoCommand, UpdateUserBillingInfoCommandResult>
    {
        private readonly ApplicationDbContext _context;

        public UpdateUserBillingInfoCommandHandler(ApplicationDbContext context, LocalFiletService localFiletService)
        {
            _context = context;
        }
        public async Task<UpdateUserBillingInfoCommandResult> Handle(UpdateUserBillingInfoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
                if (user == null)
                {
                    return new UpdateUserBillingInfoCommandResult
                    {
                        ErrorCode = ErrorCode.UserNotFound,
                        Errors = { "User Not Found" },
                        IsSuccess = false
                    };
                }
                //var city = "";
                //Guid? cityId = null;
                //var country = "";
                //Guid? countryId = null;


                //var districtName = "";
                //Guid? districtId = null;
                //if (request.DistrictId.HasValue)
                //{
                //    var district = await _context.Districts.Include(d => d.City).ThenInclude(c => c.Country).FirstOrDefaultAsync(d => d.Id == request.DistrictId);
                //    if (district == null)
                //    {
                //        return new UpdateUserBillingInfoCommandResult
                //        {
                //            ErrorCode = ErrorCode.NotFound,
                //            Errors = { "District Not Found" },
                //            IsSuccess = false
                //        };
                //    }
                //    city = district.City.Name;
                //    cityId = district.CityId;
                //    country = district.City.Country.Name;
                //    countryId = district.City.CountryId;
                //}

                user.BillingAlternativePhoneNumber = request.BillingAlternativePhoneNumber;
                user.BillingFatherName = request.BillingFatherName;
                user.BillingFamilyName = request.BillingFamilyName;
                user.BillingFullNameInEnglish = request.BillingFullNameInEnglish;
                user.BillingMainPhoneNumber = request.BillingMainPhoneNumber;
                user.BillingFirstName = request.BillingFirstName;




                _context.Users.Update(user);
                await _context.SaveChangesAsync();


                return new UpdateUserBillingInfoCommandResult
                {
                    IsSuccess = true,
                    User = new UserBillingInfoVM
                    {
                        BillingFirstName = user.BillingFirstName,
                        BillingMainPhoneNumber = user.BillingMainPhoneNumber,
                        BillingFullNameInEnglish = user.BillingFullNameInEnglish,
                        BillingFamilyName = user.BillingFamilyName,
                        BillingAlternativePhoneNumber = user.BillingAlternativePhoneNumber,
                        BillingFatherName = user.BillingFatherName,
                        Id = user.Id
                    }
                };
            }
            catch (Exception ex)
            {
                return new UpdateUserBillingInfoCommandResult
                {
                    ErrorCode = ErrorCode.Error,
                    Errors = { ex.Message },
                    IsSuccess = false
                };
            }

        }
    }
}

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

namespace Application.Common.User.Commands.UpdateUserContact
{

    public record UpdateUserContactCommandResult : BaseCommandResult
    {
        public UserContactDataVM User { get; set; }
    }
    public record UpdateUserContactCommand : IRequest<UpdateUserContactCommandResult>
    {
        [Required]
        public string Id { get; set; }
        //public string? City { get; set; }
        //public string? Country { get; set; }
        //public string? State { get; set; }
        public string? ZIP { get; set; }

        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Fax { get; set; }

        public Guid? DistrictId { get; set; }
    }
    public class UserContactDataVM
    {
        public string Id { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        
        
        public Guid? CityId { get; set; }
        public Guid? CountryId { get; set; }
        
        
        public string? District { get; set; }
        public Guid? DistrictId { get; set; }
        public string? ZIP { get; set; }

        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Fax { get; set; }
    }
    //public class UpdateUserContactCommandHandler : IRequestHandler<UpdateUserContactCommand, UpdateUserContactCommandResult>
    //{
    //    private readonly ApplicationDbContext _context;

    //    public UpdateUserContactCommandHandler(ApplicationDbContext context, LocalFiletService localFiletService)
    //    {
    //        _context = context;
    //    }
    //    public async Task<UpdateUserContactCommandResult> Handle(UpdateUserContactCommand request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
    //            if (user == null)
    //            {
    //                return new UpdateUserContactCommandResult
    //                {
    //                    ErrorCode = ErrorCode.UserNotFound,
    //                    Errors = { "User Not Found" },
    //                    IsSuccess = false
    //                };
    //            }
    //            var city = "";
    //            Guid? cityId = null;
    //            var country = "";
    //            Guid? countryId = null;
                
                
    //            var districtName = "";
    //            Guid? districtId = null;
    //            if (request.DistrictId.HasValue)
    //            {
    //                var district = await _context.Districts.Include(d => d.City).ThenInclude(c => c.Country).FirstOrDefaultAsync(d => d.Id == request.DistrictId);
    //                if (district == null)
    //                {
    //                    return new UpdateUserContactCommandResult
    //                    {
    //                        ErrorCode = ErrorCode.NotFound,
    //                        Errors = { "District Not Found" },
    //                        IsSuccess = false
    //                    };
    //                }
    //                city = district.City.Name;
    //                cityId = district.CityId;
    //                country = district.City.Country.Name;
    //                countryId = district.City.CountryId;
    //            }
               
    //            user.DistrictId = request.DistrictId;
    //            user.ZIP = request.ZIP;
    //            user.AddressLine1 = request.AddressLine1;
    //            user.AddressLine2 = request.AddressLine2;
    //            user.Fax = request.Fax;



    //            _context.Users.Update(user);
    //            await _context.SaveChangesAsync();


    //            return new UpdateUserContactCommandResult
    //            {
    //                IsSuccess = true,
    //                User = new UserContactDataVM
    //                {
    //                    City = city,
    //                    CityId = cityId,
    //                    Country = country,
    //                    CountryId = countryId,
    //                    Id = user.Id,
    //                    District = districtName,
    //                    DistrictId = districtId,
    //                    ZIP = user.ZIP,
    //                    Fax = user.Fax,
    //                    AddressLine1 = user.AddressLine1,
    //                    AddressLine2 = user.AddressLine2
    //                }
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new UpdateUserContactCommandResult
    //            {
    //                ErrorCode = ErrorCode.Error,
    //                Errors = { ex.Message },
    //                IsSuccess = false
    //            };
    //        }

    //    }
    //}
}

using Application.RealEstateUnit.Queries.GetAll;
using Application.Services.File;
using Application.Utilities.Extensions;
using Application.Utilities.Filter;
using Application.Utilities.Models;
using Application.Utilities.Sort;
using Domain.Models.RealEstates;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Contract.Queries.GetAll
{
    public record GetAllContractQuery : BasePaginatedQuery, IRequest<GetAllContractQueryResult>
    {
        public string? UserId { get; init; }
    }

    public record GetAllContractQueryResult : BaseCommandResult
    {
        public BasePaginatedList<ContractDto> dto { get; init; }
    }

    public record ContractDto
    {
        public long Id { get; init; }
        public string ContractNumber { get; set; }
        public string PaymentCycle { get; set; }
        public string AutomaticRenewal { get; set; }
        public string ContractRent { get; set; }
        public DateTime DateOfConclusion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public decimal? TenantTax { get; set; }
        public string Status { get; set; }
        public string ContractFile { get; set; }
        public bool IsActive { get; set; }
        public bool IsExecute { get; set; }
        public bool IsFinished { get; set; }
        public long RealEstateUnitId { get; set; }
        public long RealEstateId { get; set; }
        public long TenantId { get; set; }
        public CreatedByVM CreatedBy { get; set; }
        public CreatedByVM ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class GetAllContractQueryHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService) : IRequestHandler<GetAllContractQuery, GetAllContractQueryResult>
    {
        public async Task<GetAllContractQueryResult> Handle(GetAllContractQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var contracts = await _dbContext.Contracts
                                        .Include(c => c.RealEstateUnit)
                                        .Include(c => c.Tenant)
                                        .Search(request.SearchTerm)
                                        .Where(c => c.CreatedById == request.UserId || request.UserId == null)
                                        .Select(c => new ContractDto
                                        {
                                            Id = c.Id,
                                            StartDate = c.StartDate,
                                            EndDate = c.EndDate,
                                            AutomaticRenewal = c.AutomaticRenewal,
                                            ContractNumber = c.ContractNumber,
                                            ContractRent = c.ContractRent,
                                            DateOfConclusion = c.DateOfConclusion,
                                            Type = c.Type,
                                            IsActive = c.IsActive,
                                            IsExecute = c.IsExecute,
                                            IsFinished = c.IsFinished,
                                            PaymentCycle = c.PaymentCycle,
                                            Status = c.Status,
                                            TenantTax = c.TenantTax,
                                            RealEstateId = c.RealEstateId,
                                            RealEstateUnitId = c.RealEstateUnitId,
                                            TenantId = c.TenantId,
                                            CreatedBy = c.CreatedBy != null ? new CreatedByVM { Name = c.CreatedBy.Name, Id = c.CreatedBy.Id } : null,
                                            ModifiedBy = c.ModifiedBy != null ? new CreatedByVM { Name = c.ModifiedBy.Name, Id = c.ModifiedBy.Id } : null,
                                            CreatedDate = c.CreatedDate,
                                            ModifiedDate = c.ModifiedDate
                                        })
                                        .Filter(request.Filters)
                                        .Sort(request.Sorts ?? new List<SortedQuery>() { new SortedQuery() { PropertyName = "Number", Direction = SortDirection.ASC } })
                                        .ToPaginatedListAsync(request.PageNumber, request.PageSize);


                foreach (var contract in contracts.Items)
                {
                    var _contractFile = await _attachmentService.GetFilesUrlAsync(Path.Combine("contracts", contract.Id.ToString()));

                    if (_contractFile.IsSuccess && _contractFile.Urls.Count > 0)
                    {
                        contract.ContractFile = _contractFile.Urls[0];
                    }
                }

                return new GetAllContractQueryResult
                {
                    IsSuccess = true,
                    dto = contracts
                };
            }
            catch (Exception ex)
            {
                return new GetAllContractQueryResult
                {
                    IsSuccess = false,
                    Errors = new List<string> { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}

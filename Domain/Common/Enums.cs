using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public enum ErrorCode : byte
    {
        Error = 1,
        FieldRequired,
        MaxFieldLength,
        MinFieldLength,
        UserNotFound,
        InvalidLogin,
        DuplicateUser,
        DuplicateEmail,
        DuplicateName,
        DuplicateCRN,
        DuplicatePhoneNumber,
        NotFound,
        UserAlreadyLoggedIn,
        InvalidEmailAddress,
        InvalidPhoneNumber,
        InvalidUsername,
        Expired,
        InvalidCRN,
        InvalidPasswordRequirements,
        DuplicateIdNumber,
        InvalidStartDate,
        InvalidEndDate,
        InvalidDate,
        DuplicateCode,
        ParentNotFound,
        ProjectNotFound,
        CantLockAdmin,
        CantLockYourself,
        CantDelete,
        CompanyTypeNotEqualsProjectType,
        ExceedValue,
        CategoryNotFound,
        FileNotFound,
        BadFile,
        AccessDenied,
        LastValueIsBiggerThanCurrentValue,
        QuantityCantBeLessThanZero,
        NotEnoughItems,
        OrderCantBeUpdated,
        OrderNotApproved,
        OrderCantBeDeleted,
        UnitNotFound,
        QuantityAlreadyExist,
        ProjectStateIsLaunched,
        QuantityAlreadyReserved,
        ItemAlreadyExists,
        Duplicate,
        CantAddFinancialSummary,
        FinancialSummaryTypeMustBeFinal,
        InvalidUrl,
        AppartmentFloorLessThanBuildingFloorCount,
        CantAddAsParent,
        InvalidPercentage,
        RateMustBe5OrLess,
        QuantityMustBeGreaterThanZero,
        CantAddProduct,
        CompleteYourBillingInfo,
        CantAddAsChild,
        Unauthorized
    }


    public enum ProjectStatus
    {
        Planned,
        InProgress,
        Completed,
        OnHold,
        Cancelled
    }

    public enum UserType:byte
    {
        Homeowner=1,
        Architect,
        Designer,
        Contractor,
        Other
    }

    public enum OrderStatus : byte
    {
        Pending = 1,        // Order has been placed but not processed
        Processing = 2,     // Order is being processed
        Shipped = 3,        // Order has been shipped
        Delivered = 4,      // Order has been delivered to the customer
        Cancelled = 5,      // Order has been cancelled
        Returned = 6,       // Order has been returned by the customer
        Refunded = 7,       // Order has been refunded
        Failed = 8          // Order failed (e.g., payment issue)
    }

    public enum StatusBills
    {
        Pending = 1,          // في انتظار التحصيل
        Collected,            // تم التحصيل
        NotCollected,         // لم يتم التحصيل
        Deferred,             // مؤجل
        PartiallyCollected    // تم التحصيل جزئيا
    }

}

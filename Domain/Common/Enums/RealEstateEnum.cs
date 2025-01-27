using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Enums
{
    public class RealEstateEnum
    {
        public enum RealEstateType
        {
            House,
            Apartment,
            Office,
            Land,
            Store,
            Warehouse,
            CommercialVilla,
            PublicBenefiMarket,
            Duplex,
            Architecture,
            CommercialBuilding,
            tower,
            palace,
            Apartments,
            EducationalBuilding,
            hotel,
            FurnishedApartments
        }

        public enum RealEstateCategory
        {
            Commercial,
            Residential,
            ResidentialCommercial,
            Administrative,
            ResidentialAdministrative,
            Industrial,
            Agricultural
        }

        public enum RealEstateService
        {
            RentOwnedRealEstate,
            RentReinvestment,
            ManagementPropertyOfOthers
        }
    }
}

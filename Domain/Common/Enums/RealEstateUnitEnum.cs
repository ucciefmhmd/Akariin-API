using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Enums
{
    public class RealEstateUnitEnum
    {
        public enum RealEstateUnitRooms
        {
            ARoomWithoutAHall,
            RoomAndHall,
            FiveRoomsAndAHall,
            TwoRoomsWithoutAHall,
            ThreeRoomsWithoutAHall,
            FourRoomsWithoutAHall,
            FiveRoomsWithoutAHall,
            KitchenAndBathroom,
            RoomHallAndBathroom,
            TwoRoomsAndBathroom,
            FurnishedRoomAndHall,
            ThreeRoomsTwoBathroomsKitchenAndHall
        }

        public enum RealEstateUnitType
        {
            Apartment,
            store,
            showroom,
            office,
            TurnAround,
            Studio,
            ApartmentAndAnnex,
            shop,
            kitchen,
            Workshop,
            Cafe,
            Maintained,
            depot,
            Coop,
            Lounge,
            passageway,
            courtyard,
            vella,
        }
    }
}

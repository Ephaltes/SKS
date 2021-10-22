using System.Runtime.Serialization;

namespace NLSL.SKS.Pacakge.DataAccess.Entities.Enums
{
        /// <summary>
        /// State of the parcel.
        /// </summary>
        /// <value>State of the parcel.</value>
        public enum StateEnum
        {
        /// <summary>
        /// Enum PickupEnum for Pickup
        /// </summary>
        [EnumMember(Value = "Pickup")]
        Pickup= 0,

        /// <summary>
        /// Enum InTransportEnum for InTransport
        /// </summary>
        [EnumMember(Value = "InTransport")]
        InTransport = 1,

        /// <summary>
        /// Enum InTruckDeliveryEnum for InTruckDelivery
        /// </summary>
        [EnumMember(Value = "InTruckDelivery")]
        InTruckDelivery = 2,

        /// <summary>
        /// Enum TransferredEnum for Transferred
        /// </summary>
        [EnumMember(Value = "Transferred")]
        Transferred = 3,

        /// <summary>
        /// Enum DeliveredEnum for Delivered
        /// </summary>
        [EnumMember(Value = "Delivered")]
        Delivered = 4
    }
}
using System;

namespace DAL.Versions.V1.Entities
{
    public class TicketStore
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public int TicketTypeId { get; set; }
        public int TicketPrice { get; set; }
        public int TotalPunches { get; set; }
    }
}

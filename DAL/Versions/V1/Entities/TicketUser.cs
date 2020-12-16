using System;

namespace DAL.Versions.V1.Entities
{
    public class TicketUser
    {
        public long? Id { get; set; }
        public long UserId { get; set; }
        public long TicketStoreId { get; set; }
        public int? Punch { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastPunching { get; set; }
    }
}

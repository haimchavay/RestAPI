using System;

namespace DAL.Versions.V1.Entities
{
    public class PunchHistory
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public long TicketStoreId { get; set; }
        public long TicketUserId { get; set; }
        public int? Punch { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

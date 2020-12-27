using System;

namespace BLL.Versions.V1.DataTransferObjects
{
    public class TicketUserDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long TicketStoreId { get; set; }
        public int? Punch { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastPunching { get; set; }
        public int? TempCode { get; set; }
        public DateTime? CreatedTempCode { get; set; }
    }
}

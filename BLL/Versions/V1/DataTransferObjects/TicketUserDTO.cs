﻿using System;

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
        public int TotalPunches { get; set; }
        public string StoreName { get; set; }
        public int TicketTypeId { get; set; }
        public long PunchValue { get; set; }
        public string GiftDescription { get; set; }
    }
}

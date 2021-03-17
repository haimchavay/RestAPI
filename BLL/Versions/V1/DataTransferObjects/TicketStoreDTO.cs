namespace BLL.Versions.V1.DataTransferObjects
{
    public class TicketStoreDTO
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public int TicketTypeId { get; set; }
        public int TicketPrice { get; set; }
        public int TotalPunches { get; set; }
        public long PunchValue { get; set; }
    }
}

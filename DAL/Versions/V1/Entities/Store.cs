namespace DAL.Versions.V1.Entities
{
    public class Store
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string ContactMan { get; set; }
        public string ContactMan2 { get; set; }
        public int StoreTypeId { get; set; }
        public long UserId { get; set; }
        public string PhotoPath { get; set; }
        public string Description { get; set; }
    }
}

﻿namespace DAL.Entities
{
    public class Store
    {
        public long Id { get; set; }
        public string Store_Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Zip_Code { get; set; }
        public string Secret_Info { get; set; }
    }
}

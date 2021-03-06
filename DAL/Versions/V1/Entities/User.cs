﻿using System;

namespace DAL.Versions.V1.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastVisited { get; set; }
        public int? UserTypeId { get; set; }
        public long? FacebookSocialId { get; set; }
        public long? GmailSocialId { get; set; }
        public string PhotoPath { get; set; }
        public int? TempCode { get; set; }
        public DateTime? CreatedTempCode { get; set; }
    }
}

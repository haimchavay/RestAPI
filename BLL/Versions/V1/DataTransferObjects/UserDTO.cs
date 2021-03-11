using System;

namespace BLL.Versions.V1.DataTransferObjects
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime? LastVisited { get; set; }
        public int? UserTypeId { get; set; }
        public string PhotoPath { get; set; }
    }
}

namespace DAL.Versions.V1.Entities
{
    public class PasswordRecovery
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public int? TempCode { get; set; }
    }
}

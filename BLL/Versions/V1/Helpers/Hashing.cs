using BC = BCrypt.Net.BCrypt;

namespace BLL.Versions.V1.Helpers
{
	public class Hashing
	{
		private static string GetRandomSalt()
		{
			return BC.GenerateSalt(12);
		}

		public static string HashPassword(string password)
		{
			password += "!aS@#d";
			return BC.HashPassword(password, GetRandomSalt());
		}

		public static bool ValidatePassword(string password, string correctHash)
		{
			password += "!aS@#d";
			return BC.Verify(password, correctHash);
		}
	}
}

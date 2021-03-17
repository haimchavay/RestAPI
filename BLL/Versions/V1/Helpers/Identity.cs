using System.Security.Claims;
using System.Security.Principal;

namespace BLL.Versions.V1.Helpers
{
    public class Identity
    {
        public static string GetValueFromClaim(IIdentity userIdentity, string key)
        {
            string value = null;
            if (userIdentity is ClaimsIdentity identity)
            {
                //IEnumerable<Claim> claims = identity.Claims;
                value = identity.FindFirst(key).Value;
            }

            return value;
        }
    }
}

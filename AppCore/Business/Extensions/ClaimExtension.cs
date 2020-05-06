using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AppCore.Entities.Concretes.Identity;

namespace AppCore.Business.Extensions
{
    public static class ClaimExtension
    {
        public static void AddUserName(this IList<Claim> claims, string userName)
        {
            claims.Add(new Claim(ClaimTypes.Name, userName));
        }

        public static void AddRoleNames(this IList<Claim> claims, string[] roleNames)
        {
            roleNames.ToList().ForEach(roleName => claims.Add(new Claim(ClaimTypes.Role, roleName)));
        }

        public static void AddUserNameIdentifier(this IList<Claim> claims, string userNameIdentifier)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userNameIdentifier));
        }

        public static void AddClaim(this IList<Claim> claims, IdentityClaim claimItem)
        {
            claims.Add(new Claim(claimItem.Type, claimItem.Value));
        }

        public static void AddClaims(this IList<Claim> claims, List<IdentityClaim> claimList)
        {
            claims = claimList.Select(e => new Claim(e.Type, e.Value)).ToList();
        }
    }
}

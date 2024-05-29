using DGIIT.Authentication;
using System.Linq;
using System.Security.Claims;

namespace EventosCeremonial
{
	public static class ClaimsPrincipalExtensions
	{
		public static string GetRol(this ClaimsPrincipal user)
		{
			return (user.Identity as ClaimsIdentity)?.GetRol();
		}
		public static string GetRol(this ClaimsIdentity identity)
		{
			return identity?.GetClaimValue(ExtendedClaimTypes.Role);
		}

		public static string GetDisplayName(this ClaimsPrincipal user)
		{
			return (user.Identity as ClaimsIdentity)?.GetDisplayName();
		}

		public static string GetDisplayName2(this ClaimsPrincipal user)
		{
			return (user.Identity as ClaimsIdentity)?.GetDisplayName2();
		}

		public static string GetDisplayName2(this ClaimsIdentity identity)
		{
			return identity?.GetClaimValue(ExtendedClaimTypes.AppUserName);
		}
		public static string GetCuil(this ClaimsPrincipal user)
		{
			return (user.Identity as ClaimsIdentity)?.GetCuil();
		}
		public static string GetCuil(this ClaimsIdentity identity)
		{
			return identity?.GetClaimValue(ExtendedClaimTypes.Cuil);
		}
		public static string GetEmail(this ClaimsPrincipal user)
		{
			return (user.Identity as ClaimsIdentity)?.GetEmail();
		}
		public static string GetEmail(this ClaimsIdentity identity)
		{
			return identity?.GetClaimValue(ExtendedClaimTypes.Email);
		}
		public static string GetUserId(this ClaimsPrincipal user)
		{
			return (user.Identity as ClaimsIdentity)?.GetUserId();
		}
		public static string GetUserId(this ClaimsIdentity identity)
		{
			return identity?.GetClaimValue(ExtendedClaimTypes.UniqueId);
		}
		private static string GetClaimValue(this ClaimsIdentity identity, string tipo)
		{
			return identity?.Claims?.FirstOrDefault(c => c.Type.Equals(tipo))?.Value;
		}
	}
}
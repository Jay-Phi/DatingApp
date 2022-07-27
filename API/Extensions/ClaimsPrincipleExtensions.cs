using System;
using System.Security.Claims;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace API.Extensions
{
	public static class ClaimsPrincipleExtensions
	{
		public static string GetUsername(this ClaimsPrincipal user)
        {
			//Need the token authicated to be sure that the user who updates its profile is the currentUser AND is already logged in
			//  =>FindFirst the value of claim of the name identifier or username authenticated
			return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
	}
}

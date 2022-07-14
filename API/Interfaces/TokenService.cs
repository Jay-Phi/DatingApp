using System;
using API.Entities;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace API.Interfaces
{

	public interface ITokenService
	{
		string CreateToken(AppUser user);
	}
}

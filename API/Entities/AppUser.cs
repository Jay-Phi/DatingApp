using System;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace API.Entities
{
	public class AppUser
	{
		public int Id { get; set; }	
		public string UserName { get; set; }
		public byte[] PasswordHash { get; set; }
		public byte[] PasswordSalt { get; set; }
	}

}


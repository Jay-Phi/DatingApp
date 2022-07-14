using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace API.DTOs
{
	public class LoginDto
	{
        [Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }

		public LoginDto()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}

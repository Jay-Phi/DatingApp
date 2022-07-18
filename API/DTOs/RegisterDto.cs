using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace API.DTOs
{
	public class RegisterDto
	{
        [Required]
		public string Username { get; set; }

		[Required]
        [StringLength(10, MinimumLength = 4)]
		public string Password { get; set; }

		public RegisterDto()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}

using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace API.DTOs
{
	public class UserDto
	{
        [Required]
		public string Username { get; set; }

		[Required]
		public string Token { get; set; }

		public string PhotoUrl { get; set;}

		public UserDto()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}

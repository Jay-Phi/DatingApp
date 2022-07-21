using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace API.DTOs
{
	public class PhotoDto
	{
		public int Id { get; set; }

		public string Url { get; set; }

		public bool IsMain { get; set; }

	}
}

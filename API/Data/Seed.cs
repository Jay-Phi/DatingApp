using System;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using API.Entities;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace API.Data
{
	public class Seed
	{
		public static async Task SeedUsers(DataContext context)
        {
			//If there are already any users in the database
			if (await context.Users.AnyAsync()) return;

			var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
			var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

			foreach (var user in users)
            {
				using var hmac = new HMACSHA512();

				user.UserName = user.UserName.ToLower();
				user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("P@$$w0rd"));
				user.PasswordSalt = hmac.Key;

				context.Users.Add(user);
			}


			await context.SaveChangesAsync();
        }
	}
}

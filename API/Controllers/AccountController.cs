using System;
using API.Data;
using API.Interfaces;
using API.Entities;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;


/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace API.Controllers
{
	public class AccountController : BaseApiController
	{
		private readonly DataContext _context;

		private readonly ITokenService _tokenService;

		//Ci-dessous, comme les requêtes sur les données d'utilisateurs vont être fréquentes
		//	et que l'on souhaite vérifier si les utilisateurs appelées par les requêtes existent
		private async Task<bool> UserExists(string username)
        {
			return await _context.Users.AnyAsync( x => x.UserName == username.ToLower());
        }

		public AccountController(DataContext context, ITokenService tokenService)
		{
			_context = context;
			_tokenService = tokenService;
		}

        [HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register (RegisterDto registerDto)
        {
			if (await UserExists(registerDto.Username))
            {
				return BadRequest("Username is taken");
            }

			//Ci-dessus on appel un DTO pour éviter d'inscrire des données en input, dans l'URL.
			//	et d'avoir une exception pour le pwd
			//  et de cibler les données de l'objets

			using var hmac = new HMACSHA512();

			var user = new AppUser
			{
				UserName = registerDto.Username.ToLower(),
				PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
				PasswordSalt = hmac.Key
			};

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return new UserDto
            {
				Username = user.UserName,
				Token = _tokenService.CreateToken(user)
            };
		}

        [HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
			//Ci-dessouson effectue un select
            //	Et on vérifie s'il y a plusieurs users inscrits en base
            //	si oui une exception est envoyée
			var user = await _context.Users
				.Include(p => p.Photos)//Cas où il n'y a pas de photo
				.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
			
			if (user == null)
            {
				return Unauthorized("Invalid username");
            }

			//Get the encryption key
			using var hmac = new HMACSHA512(user.PasswordSalt);

			//Encrypt the password inputed
			var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

			for (int i = 0; i<computedHash.Length; i++)
            {
				//Compare the encrypted result
				if (computedHash[i] != user.PasswordHash[i])
                {
					return Unauthorized("Invalid password");
                }
            }

			return new UserDto
            {
				Username = user.UserName,
				Token = _tokenService.CreateToken(user),
				PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
            };
		}
	}

}

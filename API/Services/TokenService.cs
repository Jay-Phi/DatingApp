using System;
using API.Entities;
using API.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

/// <summary>
///		This class or service aims to manage the creation of the authentication token known as JWT or JSON Web Tokens
/// </summary>
/// 
namespace API.Services { 

	public class TokenService : ITokenService
	{
		private readonly SymmetricSecurityKey _key;

		public TokenService(IConfiguration config) {
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
		}
		public string CreateToken(AppUser user)
		{
			/*
			 * Structure of the JWT
			 * 
			 * Header : include the algorithm used (alg) and the type of token (typ)
			 * 
			 * Payload: built by a claim (who is the user who tries to authenticate) and credentials (username + password)
			 *			It contains the claim (nameid, role), the starting date of validity('nbf' or 'not before'), the expiring date ('exp'), the issuing date ('iat' which stands for 'issued at') 
			 *
			 * Signature: Part that is encrypted by the server by using a secure key that never leaves the server
			 *				
			 * 
			 */


			//Adding our claims
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
			};

			//Creating some credentials
			var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

			//Describing the structure of the token
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(7),
				SigningCredentials = creds

			};

			//Creation of the handler to get the original token from the server
			var tokenHandler = new JwtSecurityTokenHandler();

			//Creating the token to respond to the server
			var token = tokenHandler.CreateToken(tokenDescriptor);

			//Returning the token
			return tokenHandler.WriteToken(token);
		}
	}
}

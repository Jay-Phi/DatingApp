using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces;
using API.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
	public class UsersController : BaseApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public UsersController(IUserRepository userRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		[HttpGet]
        //[AllowAnonymous]
		public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers ()
		{
			var users = await _userRepository.GetMembersAsync();

			//var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

			return Ok(users);
		}

        //api/Users/[from 0 to 4 or 5]
		//[Authorize]
		[HttpGet("{username}")]
		public async Task<ActionResult<MemberDto>> GetUser (string username)
		{
			var user = await _userRepository.GetMemberAsync(username);
			return _mapper.Map<MemberDto>(user);
		}

        [HttpPut]
		public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
			//Need the token authicated to be sure that the user who updates its profile is the currentUser AND is already logged in
			//  =>FindFirst the value of claim of the name identifier or username authenticated
			var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			//Get the app user object
			var user = await _userRepository.GetUserByUsernameAsync(username);

			_mapper.Map(memberUpdateDto, user);

			_userRepository.Update(user);

			if (await _userRepository.SaveAllAsync())
            {
				return NoContent();
            }

			return BadRequest("Failed to update user");
        }
	}

}

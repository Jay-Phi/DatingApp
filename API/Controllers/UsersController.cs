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
	}

}

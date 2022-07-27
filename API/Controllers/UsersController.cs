using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces;
using API.DTOs;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    [Authorize]
	public class UsersController : BaseApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IPhotoService _photoService;

		public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
		{
			_userRepository = userRepository;
			_mapper = mapper;
			_photoService = photoService;
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
		[HttpGet("{username}", Name = "GetUser")]
		public async Task<ActionResult<MemberDto>> GetUser (string username)
		{
			var user = await _userRepository.GetMemberAsync(username);
			return _mapper.Map<MemberDto>(user);
		}

        [HttpPut]
		public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
			//Need the token authicated to be sure that the user who updates its profile is the currentUser AND is already logged in
			//  method GetUserName() => use FindFirst the value of claim of the name identifier or username authenticated
			//	Then Get the app user object
			var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

			_mapper.Map(memberUpdateDto, user);

			_userRepository.Update(user);

			if (await _userRepository.SaveAllAsync())
            {
				return NoContent();
            }

			return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
		public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
			var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        
			var result = await _photoService.AddPhotoAsync(file);

			if(result.Error != null)
            {
				return BadRequest(result.Error.Message);
            }

			var photo = new Photo
            {
				Url = result.SecureUrl.AbsoluteUri,
				PublicId = result.PublicId
            };

			if (user.Photos.Count == 0)
            {
				photo.IsMain = true;
            }

			user.Photos.Add(photo);

			if (await _userRepository.SaveAllAsync())
            {
				//return _mapper.Map<PhotoDto>(photo);
				return CreatedAtRoute("GetUser", new {username = user.UserName }, _mapper.Map<PhotoDto>(photo));
            }

			return BadRequest("Problem adding photo");
		}

		//Change the main photo
        [HttpPut("set-main-photo/{photoId}")]
		public async Task<ActionResult> SetMainPhoto(int photoId)
        {
			//Get the current main photo
			var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
			var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

			//Check if the photo selected it is already the main photo used
			if (photo.IsMain)
            {
				return BadRequest("This is already your main photo");
            }

			//Set the photo data requested in a buffer value
			var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

			//Check if there is any data inputed by the user
			if(currentMain != null)
            {
				currentMain.IsMain = false;
            }

			//Set the photo in the db as the main one
			photo.IsMain = true;
			if (await _userRepository.SaveAllAsync())
            {
				return NoContent();
            }

			//Case where the saving request failed
			return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
		public async Task<ActionResult> DeletePhoto(int photoId)
        {
			//Get the photo to delete
			var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
			var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

			//Control if the photo inputed exists
			if (photo == null)
            {
				return NotFound();
            }
			//Control if it is not the main photo
			if (photo.IsMain)
            {
				return BadRequest("You cannot delete your main photo");
            }

			//Control if the photo has a public id and are stored in the cloud
			if (photo.PublicId != null)
            {
				//Delete the photo in the cloud
				var result = await _photoService.DeletePhotoAsync(photo.PublicId);
				//Control the result of the deletion
				if (result.Error != null)
                {
					return BadRequest(result.Error.Message);
                }
            }

			//Remove the photo from the database
			user.Photos.Remove(photo);
			if (await _userRepository.SaveAllAsync())
            {
				return Ok();//Message returned if the removal succeeds
            }
			//Message returned if the removal fails
			return BadRequest("Failed to delete the photo");
        }
	}

}

using System;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

	[ApiController]
	[Route("[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly DataContext _context;

		public UsersController(DataContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers ()
		{
			return await _context.Users.ToListAsync();
		}
		
		//API/Users/[from 0 to 4 or 5]
		[HttpGet("{id}")]
		public async Task<ActionResult<AppUser>> GetUser (int id)
		{
			return await _context.Users.FindAsync(id);
		}
	}

}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarFix.Data;
using SolarFix.DTO;
using SolarFix.Entities;

namespace SolarFix.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class UserController(AppDbContext context, IMapper mapper) : ControllerBase
	{
		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult<UserResponseDTO>> AddUser([FromBody] UserDTO user)
		{
			if (user == null) return BadRequest("Enter A Correct Data.");

			var newUser = mapper.Map<User>(user);
			newUser.UserId = 0;
			newUser.CreatedAt = DateTime.Now;

			await context.Set<User>().AddAsync(newUser);

			if (await context.SaveChangesAsync() < 1) return BadRequest();

			var userToReturn = mapper.Map<UserResponseDTO>(newUser);

			return CreatedAtRoute("GetUserById", new { id = newUser.UserId }, userToReturn);
		}

		[HttpPut]
		public async Task<ActionResult<UserResponseDTO>> UpdateUser([FromBody] UserDTO user)
		{
			if (user == null) return BadRequest("Enter A Correct Data.");

			if (!await context.Set<User>().AnyAsync(u => u.UserId == user.UserId)) return NotFound();

			var uUser = mapper.Map<User>(user);
			uUser.CreatedAt = DateTime.UtcNow;

			context.Set<User>().Update(uUser);

			if (await context.SaveChangesAsync() < 1) return BadRequest();

			var userToReturn = mapper.Map<UserResponseDTO>(uUser);

			return Ok(uUser);
		}

		[HttpGet("{id}", Name = "GetUserById")]
		public async Task<ActionResult<UserResponseDTO>> GetUserById(int id)
		{
			var user = await context.Set<User>().FirstOrDefaultAsync(u => u.UserId == id);

			if (user == null) return NotFound();

			return Ok(mapper.Map<UserResponseDTO>(user));
		}

		[HttpHead("{id}")]
		public async Task<ActionResult> IsUserExists(int id)
		{
			if (id > int.MaxValue) return BadRequest();

			return await context.Set<User>().AnyAsync(u => u.UserId == id) ? Ok() : NotFound();
		}

		[HttpGet]
		public ActionResult<IEnumerable<UserResponseDTO>> GetAllUsers()
		{
			var users = context.Set<User>();

			if (users == null) return NotFound();

			return Ok(mapper.Map<IEnumerable<UserResponseDTO>>(users));
		}

	}
}

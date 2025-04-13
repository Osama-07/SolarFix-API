using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarFix.Data;
using SolarFix.DTO;
using SolarFix.Entities;
using SolarFix.Services;

namespace SolarFix.Controllers
{
	[Route("api/")]
	[ApiController]
	public class LoginController(AppDbContext context, IMapper mapper, JwtTokenService jwtTokenService) : ControllerBase
	{
		[HttpPost("Login", Name = "Login")]
		[AllowAnonymous]
		public async Task<ActionResult<UserResponseDTO>> Login([FromBody] LoginDTO loginInfo)
		{
			if (string.IsNullOrEmpty(loginInfo.Email) ||
				string.IsNullOrEmpty(loginInfo.Password))
				return BadRequest();

			var user = await context.Set<User>().FirstOrDefaultAsync(u => u.Email == loginInfo.Email && u.Password == loginInfo.Password);

			if (user == null)
				return NotFound();

			var token = jwtTokenService.GenerateToken(user.Email);

			Response.Headers.Append("Authorization", $"Bearer {token}");

			var userDto = mapper.Map<UserResponseDTO>(user);

			return Ok(userDto);
		}

		[HttpPost("SignUp", Name = "SignUp")]
		[AllowAnonymous]
		public async Task<ActionResult> SignUp([FromBody] SignUpDTO sign)
		{
			if (sign == null)
				return BadRequest("Enter Correct Info.");

			if (await context.Users.AnyAsync(u => u.Email == sign.Email))
				return Conflict("This Email Is Already Exists.");

			// Add User
			var newUser = mapper.Map<User>(sign);
			newUser.UserId = 0;

			await context.Set<User>().AddAsync(newUser);

			if (await context.SaveChangesAsync() < 1)
				return BadRequest("Add User Is Failed.");

			if (sign.UserType == Enums.enUserType.Technician)
			{
				var newTechnician = mapper.Map<Technician>(sign);
				newTechnician.TechnicianId = 0;
				newTechnician.UserId = newUser.UserId;
				newTechnician.Rating = 5; // 5 Rate Is Default.

				await context.Technicians.AddAsync(newTechnician);

				if (await context.SaveChangesAsync() < 1)
					return BadRequest("Add Technician Is Failed.");
			}

			return Ok("Added Successfully.");
		}
	}
}

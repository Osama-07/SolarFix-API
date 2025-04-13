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
	public class TechnicianController(AppDbContext context, IMapper mapper) : ControllerBase
    {
		[HttpPost]
		public async Task<ActionResult<TechnicianDTO>> AddTechnician([FromBody] TechnicianDTO technician)
		{
			if (technician == null) return BadRequest("Enter A Correct Data.");

			var newTechnician = mapper.Map<Technician>(technician);
			newTechnician.TechnicianId = 0;

			await context.Set<Technician>().AddAsync(newTechnician);

			if (await context.SaveChangesAsync() < 1)
				return BadRequest("Cannot Add Technician Info, Try Again Later.");

			var TechnicianToReturn = mapper.Map<TechnicianDTO>(newTechnician);

			return CreatedAtRoute("GetTechnicianById", new { id = newTechnician.TechnicianId }, TechnicianToReturn);
		}

		[HttpPut]
		public async Task<ActionResult<TechnicianDTO>> UpdateTechnician([FromBody] TechnicianDTO technician)
		{
			if (technician == null) return BadRequest("Enter A Correct Data.");

			if (!await context.Set<Technician>().AnyAsync(u => u.TechnicianId == technician.TechnicianId)) return NotFound();

			var uTechnician = mapper.Map<Technician>(technician);

			context.Set<Technician>().Update(uTechnician);

			if (await context.SaveChangesAsync() < 1) return BadRequest();

			var TechnicianToReturn = mapper.Map<TechnicianDTO>(uTechnician);

			return Ok(uTechnician);
		}

		[HttpGet("{id}", Name = "GetTechnicianById")]
		public async Task<ActionResult<TechnicianDTO>> GetTechnicianById(int id)
		{
			var technician = await context.Set<Technician>().FirstOrDefaultAsync(u => u.TechnicianId == id);

			if (technician == null) return NotFound();

			return Ok(mapper.Map<TechnicianDTO>(technician));
		}

		[HttpHead("{id}")]
		public async Task<ActionResult> IsTechnicianExists(int id)
		{
			if (id > int.MaxValue) return BadRequest();

			return await context.Set<Technician>().AnyAsync(u => u.TechnicianId == id) ? Ok() : NotFound();
		}

		[HttpGet]
		public ActionResult<IEnumerable<TechnicianDetailsDTO>> GetAllTechnicians()
		{
			var technicians = context.Technicians
									 .Include(t => t.User)
									 .ToList();

			if (technicians.Count == 0 || technicians == null) return NotFound();

			var dto = mapper.Map<IEnumerable<TechnicianDetailsDTO>>(technicians);

			return Ok(dto);
		}
	}
}

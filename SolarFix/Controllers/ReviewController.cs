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
	public class ReviewController(AppDbContext context, IMapper mapper) : ControllerBase
    {
		[HttpPost]
		public async Task<ActionResult<ReviewDTO>> AddReview([FromBody] ReviewRequestDTO review)
		{
			if (review == null) return BadRequest("Enter A Correct Data.");

			if (await context.Reviews.AnyAsync(r => r.OrderId == review.OrderId))
				return BadRequest("You Aleardy Reviewd This Order.");

			var newReview = mapper.Map<Review>(review);
			newReview.ReviewId = 0;

			await context.Set<Review>().AddAsync(newReview);

			// Save to ensure the new review is included in calculations
			if (await context.SaveChangesAsync() < 1)
				return BadRequest();

			// Change Technician Rating.
			int technicianId = await context.Orders
									  .Where(o => o.OrderId == newReview.OrderId)
									  .Select(o => o.TechnicianId)
									  .FirstAsync();

			var averageRate = await context.Reviews.Where(r => r.Order.TechnicianId == technicianId)
												   .AverageAsync(r => r.Rating);


			var technician = await context.Technicians.FirstOrDefaultAsync(t => t.TechnicianId == technicianId);
			technician!.Rating = averageRate;

			if (await context.SaveChangesAsync() < 1)
				return BadRequest("Update Rating On Technician Is Failed.");

			// Return New Review Info.
			var ReviewToReturn = mapper.Map<ReviewDTO>(newReview);

			return CreatedAtRoute("GetReviewById", new { id = newReview.ReviewId }, ReviewToReturn);
		}

		[HttpPut]
		public async Task<ActionResult<ReviewDTO>> UpdateReview([FromBody] ReviewRequestDTO review)
		{
			if (review == null) return BadRequest("Enter A Correct Data.");

			if (!await context.Set<Review>().AnyAsync(u => u.ReviewId == review.ReviewId)) return NotFound();

			var uReview = mapper.Map<Review>(review);

			context.Set<Review>().Update(uReview);

			if (await context.SaveChangesAsync() < 1) return BadRequest();

			var ReviewToReturn = mapper.Map<ReviewDTO>(uReview);

			return Ok(uReview);
		}

		[HttpGet("{id}", Name = "GetReviewById")]
		public async Task<ActionResult<ReviewDTO>> GetReviewById(int id)
		{
			var review = await context.Set<Review>().FirstOrDefaultAsync(u => u.ReviewId == id);

			if (review == null) return NotFound();

			return Ok(mapper.Map<ReviewDTO>(review));
		}

		[HttpHead("{id}")]
		public async Task<ActionResult> IsReviewExists(int id)
		{
			if (id > int.MaxValue) return BadRequest();

			return await context.Set<Review>().AnyAsync(u => u.ReviewId == id) ? Ok() : NotFound();
		}

		[HttpGet]
		public ActionResult<IEnumerable<ReviewDTO>> GetAllReviews()
		{
			var reviews = context.Set<Review>();

			if (reviews == null) return NotFound();

			return Ok(mapper.Map<IEnumerable<ReviewDTO>>(reviews));
		}
	}
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarFix.Data;
using SolarFix.DTO;
using SolarFix.Entities;
using SolarFix.Enums;

namespace SolarFix.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class OrderController(AppDbContext context, IMapper mapper) : ControllerBase
	{
		[HttpPost]
		public async Task<ActionResult<OrderDTO>> AddOrder([FromBody] OrderRequestDTO order)
		{
			if (order == null) return BadRequest("Enter A Correct Data.");

			if (await context.Orders.AnyAsync(o => o.CustomerId == order.CustomerId && o.Status != enOrderStatus.Completed))
				return Conflict(); // return 409.

			var newOrder = mapper.Map<Order>(order);
			newOrder.OrderId = 0;

			await context.Set<Order>().AddAsync(newOrder);

			if (await context.SaveChangesAsync() < 1) return BadRequest();

			var OrderToReturn = mapper.Map<OrderDTO>(newOrder);

			return CreatedAtRoute("GetOrderById", new { id = newOrder.OrderId }, OrderToReturn);
		}

		[HttpPut]
		public async Task<ActionResult<OrderDTO>> UpdateOrder([FromBody] OrderDTO order)
		{
			if (order == null) return BadRequest("Enter A Correct Data.");

			if (!await context.Set<Order>().AnyAsync(u => u.OrderId == order.OrderId)) return NotFound();

			var uOrder = mapper.Map<Order>(order);
			uOrder.CreatedAt = DateTime.Now;

			context.Set<Order>().Update(uOrder);

			if (await context.SaveChangesAsync() < 1) return BadRequest();

			var OrderToReturn = mapper.Map<OrderDTO>(uOrder);

			return Ok(uOrder);
		}

		[HttpPut("SetAccepted/{id}", Name = "SetAccepted")]
		public async Task<ActionResult<OrderDTO>> SetAccepted(int id)
		{
			if (id < 1 || id > int.MaxValue) return BadRequest("Enter A Correct Data.");

			var order = await context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);

			if (order == null)
				return NotFound();

			order.Status = enOrderStatus.Accepted;
			context.Set<Order>().Update(order);

			if (await context.SaveChangesAsync() < 1) return BadRequest();

			var OrderToReturn = mapper.Map<OrderDTO>(order);

			return Ok(OrderToReturn);
		}

		[HttpPut("SetCompleted/{id}", Name = "SetCompleted")]
		public async Task<ActionResult<OrderDTO>> SetCompleted(int id)
		{
			if (id < 1 || id > int.MaxValue) return BadRequest("Enter A Correct Data.");

			var order = await context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);

			if (order == null)
				return NotFound();

			order.Status = enOrderStatus.Completed;
			context.Set<Order>().Update(order);

			if (await context.SaveChangesAsync() < 1) return BadRequest();

			var OrderToReturn = mapper.Map<OrderDTO>(order);

			return Ok(OrderToReturn);
		}

		[HttpGet("{id}", Name = "GetOrderById")]
		public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
		{
			var order = await context.Set<Order>().FirstOrDefaultAsync(u => u.OrderId == id);

			if (order == null) return NotFound();

			return Ok(mapper.Map<OrderDTO>(order));
		}

		[HttpHead("{id}")]
		public async Task<ActionResult> IsOrderExists(int id)
		{
			if (id > int.MaxValue) return BadRequest();

			return await context.Set<Order>().AnyAsync(u => u.OrderId == id) ? Ok() : NotFound();
		}

		[HttpGet]
		public ActionResult<IEnumerable<OrderDTO>> GetAllOrders()
		{
			var orders = context.Set<Order>();

			if (orders == null) return NotFound();

			return Ok(mapper.Map<IEnumerable<OrderDTO>>(orders));
		}

		[HttpGet("CustomerOrders/{id}")]
		public async Task<ActionResult<IEnumerable<OrderDetailsDTO>>> GetAllCustomerOrders(int id)
		{
			if (id < 1 || id > int.MaxValue) return BadRequest();

			var orders = await context.Orders.Include(o => o.Customer)
											 .Include(o => o.Technician)
												.ThenInclude(t=>t.User)
											 .Include(o => o.Review)
											 .Where(o => o.CustomerId == id)
											 .ToListAsync();

			if (orders == null || orders.Count == 0) return NotFound();

			var dto = mapper.Map<IEnumerable<OrderDetailsDTO>>(orders);

			return Ok(dto);
		}

		[HttpGet("TechnicianOrders/{id}")]
		public async Task<ActionResult<IEnumerable<OrderDetailsDTO>>> GetAllTechnicianOrders(int id)
		{
			if (id < 1 || id > int.MaxValue) return BadRequest();

			var orders = await context.Orders.Include(o => o.Customer)
											 .Include(o => o.Technician)
												.ThenInclude(t => t.User)
											 .Include(o => o.Review)
											 .Where(o => o.Technician.UserId == id)
											 .ToListAsync();

			if (orders == null || orders.Count == 0) return NotFound();

			var dto = mapper.Map<IEnumerable<OrderDetailsDTO>>(orders);

			return Ok(dto);
		}
	}
}

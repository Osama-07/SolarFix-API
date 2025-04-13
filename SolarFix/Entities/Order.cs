using SolarFix.Enums;

namespace SolarFix.Entities
{
	public class Order
	{
		public int OrderId { get; set; }
		public int TechnicianId { get; set; }
		public int CustomerId { get; set; }
		// 0 = Pending, 1 = Accepted, 2 = Completed.
		public enOrderStatus Status { get; set; }
		public DateTime CreatedAt { get; set; }

		public Technician Technician { get; set; } = null!;
		public User Customer { get; set; } = null!;
		public Review? Review { get; set; }
	}
}

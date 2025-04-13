namespace SolarFix.DTO
{
	public class OrderDTO
	{
		public int OrderId { get; set; }
		public int TechnicianId { get; set; }
		public int CustomerId { get; set; }
		// 0 = Pending, 1 = Accepted, 2 = Completed.
		public string Status { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
	}
}

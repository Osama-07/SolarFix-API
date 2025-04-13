namespace SolarFix.DTO
{
	public class OrderDetailsDTO
	{
		public int OrderId { get; set; }
		public int TechnicianId { get; set; }
		public string TechnicianName { get; set; } = null!;
		public int ExperienceYears { get; set; }
		public double PricePerHour { get; set; }
		public string TechnicianEmail { get; set; } = null!;
		public int CustomerId { get; set; }
		public string CustomerName { get; set; } = null!;
		public string CustomerEmail { get; set; } = null!;
		public string Status { get; set; } = null!;
		public bool IsRated { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}

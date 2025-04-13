namespace SolarFix.Entities
{
	public class Technician
	{
		public int TechnicianId { get; set; }
		public int UserId { get; set; }
		public int ExperienceYears { get; set; }
		public double PricePerHour { get; set; }
		public double Rating { get; set; }

		public User User { get; set; } = null!;
		public ICollection<Order> Orders { get; set; } = [];
	}
}

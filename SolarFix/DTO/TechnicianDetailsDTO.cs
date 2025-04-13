namespace SolarFix.DTO
{
	public class TechnicianDetailsDTO
	{
		public int TechnicianId { get; set; }
		public string FullName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Phone { get; set; } = null!;
		public int ExperienceYears { get; set; }
		public double PricePerHour { get; set; }
		public double Rating { get; set; }
	}
}

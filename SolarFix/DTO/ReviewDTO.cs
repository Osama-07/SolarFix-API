namespace SolarFix.DTO
{
	public class ReviewDTO
	{
		public int ReviewId { get; set; }
		public int OrderId { get; set; }
		public byte Rating { get; set; } // 1 => 5.
		public string? Comment { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}

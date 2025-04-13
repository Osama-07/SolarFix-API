namespace SolarFix.DTO
{
	public class ReviewRequestDTO
	{
		public int ReviewId { get; set; }
		public int OrderId { get; set; }
		public byte Rating { get; set; } // 1 => 5.
		public string? Comment { get; set; }
	}
}

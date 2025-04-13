namespace SolarFix.Entities
{
	public class Review
	{
		public int ReviewId { get; set; }
		public int OrderId { get; set; }
		public byte Rating { get; set; } // 1 => 5.
		public string? Comment { get; set; }
		public DateTime CreatedAt { get; set; }

		public Order Order { get; set; } = null!;
	}
}

namespace SolarFix.DTO
{
	public class UserResponseDTO
	{
		public int UserId { get; set; }
		public string FullName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Phone { get; set; } = null!;
		// 0 = Technician, 1 = Customer.
		public string UserType { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
	}
}

using SolarFix.Enums;

namespace SolarFix.DTO
{
	public class UserDTO
	{
		public int UserId { get; set; }
		public string FullName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string Phone { get; set; } = null!;
		// 0 = Technician, 1 = Customer.
		public enUserType UserType { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}

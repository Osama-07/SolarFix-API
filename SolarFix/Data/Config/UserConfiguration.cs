using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarFix.Entities;
using SolarFix.Enums;

namespace SolarFix.Data.Config
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users");

			builder.HasKey(u => u.UserId);

			builder.Property(u => u.UserId)
				.ValueGeneratedOnAdd();

			builder.Property(u => u.FullName)
				.HasColumnType("NVARCHAR(50)")
				.IsRequired();

			builder.Property(u => u.Email)
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(u => u.Password)
				.HasColumnType("NVARCHAR(255)")
				.IsRequired();

			builder.Property(u => u.UserType)
				.HasConversion<int>()
				.IsRequired();

			builder.Property(u => u.CreatedAt)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")  // الصحيح في SQLite
				.IsRequired();

			//builder.HasData(LoadUser());

		}

		//public static List<User> LoadUser()
		//{
		//	return
		//	[
		//		new User { FullName = "Ahmed Ali", Email = "ahmed@example.com", Password = "password123", UserType = enUserType.Customer, CreatedAt = DateTime.UtcNow },
		//		new User { FullName = "Sara Mohammed", Email = "sara@example.com", Password = "password123", UserType = enUserType.Technician, CreatedAt = DateTime.UtcNow },
		//		new User { FullName = "Mohammad Ali", Email = "mohammad@example.com", Password = "password123", UserType = enUserType.Technician, CreatedAt = DateTime.UtcNow }
		//	];
		//}
	}
}

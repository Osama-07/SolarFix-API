using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarFix.Entities;

namespace SolarFix.Data.Config
{
	public class TechnicianConfiguration : IEntityTypeConfiguration<Technician>
	{
		public void Configure(EntityTypeBuilder<Technician> builder)
		{
			builder.ToTable("Technicians");

			builder.HasKey(t => t.TechnicianId);

			builder.Property(t => t.TechnicianId)
				.ValueGeneratedOnAdd();

			builder.Property(t => t.ExperienceYears)
				.IsRequired();

			builder.Property(t => t.PricePerHour)
				.IsRequired();

			builder.Property(t => t.Rating)
				.IsRequired();

			builder.HasOne(t => t.User)
				.WithOne(u => u.Technician)
				.HasForeignKey<Technician>(t => t.UserId)
				.IsRequired();

			//builder.HasData(LoadTechnicians());
		}

		//public static List<Technician> LoadTechnicians()
		//{
		//	return
		//	[
		//		new Technician { UserId = 2, ExperienceYears = 5, PricePerHour = 50.0, Rating = 4.5 },
		//		new Technician { UserId = 3, ExperienceYears = 3, PricePerHour = 40.0, Rating = 4.0 }
		//	];
		//}

	}
}

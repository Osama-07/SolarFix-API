using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarFix.Entities;

namespace SolarFix.Data.Config
{
	public class ReviewConfiguration : IEntityTypeConfiguration<Review>
	{
		public void Configure(EntityTypeBuilder<Review> builder)
		{
			builder.ToTable("Reviews");

			builder.HasKey(r => r.ReviewId);

			builder.Property(r => r.ReviewId)
				.ValueGeneratedOnAdd();

			builder.HasOne(r => r.Order)
				.WithOne(o => o.Review)
				.HasForeignKey<Review>(r => r.OrderId)
				.IsRequired();

			builder.Property(r => r.Rating)
				.IsRequired();

			builder.Property(r => r.Comment)
				.HasColumnType("NVARCHAR(255)")
				.IsRequired(false);

			builder.Property(r => r.CreatedAt)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")  // الصحيح في SQLite
				.IsRequired();

			//builder.HasData(LoadReviews());
		}

		//public static List<Review> LoadReviews()
		//{
		//	return 
		//	[
		//		new Review { OrderId = 2, Rating = 5, Comment = "Excellent service!", CreatedAt = DateTime.Now }
		//	];
		//}
	}
}

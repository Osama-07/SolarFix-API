using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarFix.Entities;
using SolarFix.Enums;

namespace SolarFix.Data.Config
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.ToTable("Orders");

			builder.HasKey(o => o.OrderId);

			builder.Property(o => o.OrderId)
				.ValueGeneratedOnAdd();

			builder.HasOne(o => o.Technician)
				.WithMany(t => t.Orders)
				.HasForeignKey(o => o.TechnicianId)
				.IsRequired();

			builder.HasOne(o => o.Customer)
				.WithMany(u => u.Orders)
				.HasForeignKey(o => o.CustomerId)
				.IsRequired();

			builder.Property(o => o.Status)
				.HasConversion<int>()
				.IsRequired();

			builder.Property(o => o.CreatedAt)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")  // الصحيح في SQLite
				.IsRequired();

			//builder.HasData(LoadOrders());
		}

		//public static List<Order> LoadOrders()
		//{
		//	return new List<Order>
		//	{
		//		new Order { TechnicianId = 1, CustomerId = 1, Status = enOrderStatus.Pending, CreatedAt = DateTime.UtcNow },
		//		new Order { TechnicianId = 2, CustomerId = 1, Status = enOrderStatus.Completed, CreatedAt = DateTime.UtcNow }
		//	};
		//}
	}

}

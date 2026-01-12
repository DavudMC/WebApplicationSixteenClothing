using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplicationSixteenClothing.Models;

namespace WebApplicationSixteenClothing.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(1024);
            builder.Property(x => x.ImagePath).IsRequired().HasMaxLength(1024);


            builder.Property(x => x.Price).HasPrecision(10, 2);
            builder.Property(x => x.Rating).HasPrecision(2, 1);

            builder.ToTable(options =>
            {
                options.HasCheckConstraint("CK_Products_Price", "[Price]>0");
                options.HasCheckConstraint("CK_Products_Rating", "[Rating] between 0 and 5");
            });

            builder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Cascade);
        }
    }
}

namespace EFCore.AppLib.CodeFirst.Configurations
{
    using EFCore.AppLib.CodeFirst.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class InternalLogConfiguration : IEntityTypeConfiguration<InternalLog>
    {
        public void Configure(EntityTypeBuilder<InternalLog> builder)
        {   
            builder.Property(p => p.LogLevel).IsRequired().HasMaxLength(50);
            builder.Property(p => p.EventId).IsRequired();
            builder.Property(p => p.State).IsRequired();

            // builder.Property(p => p.RowTimeStamp).HasDefaultValueSql("getdate()");
            // .HasDefaultValueSql("getdate()");
            // .HasComputedColumnSql("[LastName] + ', ' + [FirstName]");
            // .HasComputedColumnSql("LEN([LastName]) + LEN([FirstName])", stored: true);
            // modelBuilder.Entity<InternalLog>().Property(b => b.RowTimeStamp).HasDefaultValueSql("getdate()");
            //builder.Property(p => p.LogLevel).IsRequired();
            //builder.Property(p => p.State).IsRequired();
            //        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            //        builder.Property(p => p.Description).IsRequired();
            //        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            //        builder.Property(p => p.PictureUrl).IsRequired();
            //        builder.HasOne(p => p.ProductBrand).WithMany()
            //            .HasForeignKey(p => p.ProductBrandId);
            //        builder.HasOne(p => p.ProductType).WithMany()
            //            .HasForeignKey(p => p.ProductTypeId);

        }
    }
}

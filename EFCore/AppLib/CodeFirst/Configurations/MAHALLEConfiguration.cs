namespace EFCore.AppLib.CodeFirst.Configurations
{
    using EFCore.AppLib.CodeFirst.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MAHALLEConfiguration : IEntityTypeConfiguration<MAHALLE>
    {
        public void Configure(EntityTypeBuilder<MAHALLE> builder)
        {
            //builder.HasIndex(e => e.MahalleKodu).IsUnique(true);
            //builder.Property(p => p.MahalleKodu).IsRequired();
        }
    }
}

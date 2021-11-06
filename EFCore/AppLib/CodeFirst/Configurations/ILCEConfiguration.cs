namespace EFCore.AppLib.CodeFirst.Configurations
{
    using EFCore.AppLib.CodeFirst.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ILCEConfiguration : IEntityTypeConfiguration<ILCE>
    {
        public void Configure(EntityTypeBuilder<ILCE> builder)
        {
            //builder.HasIndex(e => e.IlceKodu).IsUnique(true);
            //builder.Property(p => p.IlceKodu).IsRequired();
        }
    }
}

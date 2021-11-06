namespace EFCore.AppLib.CodeFirst.Configurations
{
    using EFCore.AppLib.CodeFirst.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SBBConfiguration : IEntityTypeConfiguration<SBB>
    {
        public void Configure(EntityTypeBuilder<SBB> builder)
        {
            //builder.HasIndex(e => e.SbbKodu).IsUnique(true);
            //builder.Property(p => p.SbbKodu).IsRequired();
        }
    }
}

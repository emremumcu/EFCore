namespace EFCore.AppLib.CodeFirst.Configurations
{
    using EFCore.AppLib.CodeFirst.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ILConfiguration : IEntityTypeConfiguration<IL>
    {
        public void Configure(EntityTypeBuilder<IL> builder)
        {
            //builder.HasIndex(e => e.IlKodu).IsUnique(true);
            //builder.Property(p => p.IlKodu).IsRequired();
            builder.Property(i => i.IlKodu).HasColumnType("char(2)").HasMaxLength(2);
        }
    }
}

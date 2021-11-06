﻿// <auto-generated />
using System;
using EFCore.AppLib.CodeFirst;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCore.AppLib.CodeFirst.Migrations
{
    [DbContext(typeof(CodeFirstDbContext))]
    partial class CodeFirstDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EFCore.AppLib.CodeFirst.Entities.IL", b =>
                {
                    b.Property<string>("IlKodu")
                        .HasMaxLength(2)
                        .HasColumnType("char(2)");

                    b.Property<string>("IlAdi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RowTimeStamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("IlKodu");

                    b.ToTable("Iller");
                });

            modelBuilder.Entity("EFCore.AppLib.CodeFirst.Entities.ILCE", b =>
                {
                    b.Property<string>("IlceKodu")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IlKodu")
                        .HasColumnType("char(2)");

                    b.Property<string>("IlceAdi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RowTimeStamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("IlceKodu");

                    b.HasIndex("IlKodu");

                    b.ToTable("Ilceler");
                });

            modelBuilder.Entity("EFCore.AppLib.CodeFirst.Entities.InternalLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EventId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExceptionMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InnerExceptionMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogLevel")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("RowTimeStamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InternalLogs");
                });

            modelBuilder.Entity("EFCore.AppLib.CodeFirst.Entities.MAHALLE", b =>
                {
                    b.Property<string>("MahalleKodu")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MahalleAdi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RowTimeStamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("SbbKodu")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MahalleKodu");

                    b.HasIndex("SbbKodu");

                    b.ToTable("Mahalleler");
                });

            modelBuilder.Entity("EFCore.AppLib.CodeFirst.Entities.SBB", b =>
                {
                    b.Property<string>("SbbKodu")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IlceKodu")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("RowTimeStamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("SemtBucakBeldeAdi")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SbbKodu");

                    b.HasIndex("IlceKodu");

                    b.ToTable("SemtBucakBeldeler");
                });

            modelBuilder.Entity("EFCore.AppLib.CodeFirst.Entities.ILCE", b =>
                {
                    b.HasOne("EFCore.AppLib.CodeFirst.Entities.IL", "Il")
                        .WithMany()
                        .HasForeignKey("IlKodu");

                    b.Navigation("Il");
                });

            modelBuilder.Entity("EFCore.AppLib.CodeFirst.Entities.MAHALLE", b =>
                {
                    b.HasOne("EFCore.AppLib.CodeFirst.Entities.SBB", "SemtBucakBelde")
                        .WithMany()
                        .HasForeignKey("SbbKodu");

                    b.Navigation("SemtBucakBelde");
                });

            modelBuilder.Entity("EFCore.AppLib.CodeFirst.Entities.SBB", b =>
                {
                    b.HasOne("EFCore.AppLib.CodeFirst.Entities.ILCE", "Ilce")
                        .WithMany()
                        .HasForeignKey("IlceKodu");

                    b.Navigation("Ilce");
                });
#pragma warning restore 612, 618
        }
    }
}

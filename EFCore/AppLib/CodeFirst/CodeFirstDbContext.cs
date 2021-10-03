namespace EFCore.AppLib.CodeFirst
{
    using EFCore.AppLib.CodeFirst.Entities;
    using Microsoft.EntityFrameworkCore;

    public class CodeFirstDbContext : DbContext
    {
        /// <summary>
        /// This constructor is mandatory !!!
        /// A database provider can be configured by overriding the 'DbContext.OnConfiguring' method 
        /// or by using 'AddDbContext' on the application service provider. 
        /// If 'AddDbContext' is used, then also ensure that your DbContext type accepts a DbContextOptions<TContext> object 
        /// in its constructor and passes it to the base constructor for DbContext.
        /// </summary>
        public CodeFirstDbContext()
        {
            /// Database.EnsureCreated(); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {           
            optionsBuilder.UseSqlite("Data Source=CodeFirstDB.sqlite;");

            // For development only:
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /// Maps entities to tables using specified names
            // modelBuilder.Entity<Il>().ToTable("Iller");
            // modelBuilder.Entity<Ilce>().ToTable("Ilceler");            
        }

        /// Entities
        public DbSet<IL> Iller { get; set; }
        public DbSet<ILCE> Ilceler { get; set; }
        public DbSet<SBB> SemtBucakBeldeler { get; set; }
        public DbSet<MAHALLE> Mahalleler { get; set; }
    }
}

/// *****
/// NOTES
/// *****
/// 
/// EF-Core Code First Create Database:
/// -----------------------------------
/// After creating entity classes and dbcontext run the following commands:
/// PM> add-migration Mig0 -OutputDir AppData/Migrations        (add migration with name Mig0 to output directory specified)
/// PM> update-database [name] –verbose                         (apply migration to database, if name is omitted, all migrations are updated)
/// PM> remove-migration                                        (remove last migration if it is not applied to the database)
/// PM> remove-migration [-Migration] name                      (remove the named migration if it is not applied to the database)
/// PM> update-database CreateDB                                (target migration is the point to which you want to restore the database)
/// PM> Update-Database [-Migration] 0                          (unapply all migrations and revert database back the blank state)
/// PM> Update-Database [-Migration] name                       (unapply all migrations till the given name and revert database back the previous state)
/// 
/// EF-Core DB First Create Entities:
/// ---------------------------------
/// Scaffold-DbContext "Server=.;Database=App102;User Id=sa; Password=aA123456;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir AppData/Scaffold
/// Scaffold-DbContext "Host=127.0.0.1;Database=postgres;Username=postgres;Password=aA123456" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir AppData/Scaffold

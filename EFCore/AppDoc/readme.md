# EF Core Setup

## Summary

#### This package should be installed to the project containing the DbContext
PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer -ProjectName <contextproject>

#### This package should be installed to the startup project
PM> Install-Package Microsoft.EntityFrameworkCore.Design -ProjectName <startupproject>

> Rebuild the project in Visual Studio after installing the Microsoft.EntityFrameworkCore.Design package in order to changes take effect.


#### Install & update dotnet ef CLI tools
> dotnet tool install --global dotnet-ef
> dotnet tool update --global dotnet-ef

#### Create DbContext and DbSet classes

#### Create migrations and update database
> dotnet ef migrations add InitialCreate -p <ProjectHavingDbContext> -s <StartupProject> -o EFCore/Migrations
> dotnet ef migrations add InitialCreate -p SGKWeb.Lib -s SGKWeb.CMS.UI -o EFCore/Migrations
> dotnet ef database update -p SGKWeb.Lib -s SGKWeb.CMS.UI
> dotnet ef migrations remove -p SGKWeb.Lib -s SGKWeb.CMS.UI
> dotnet ef database drop -p SGKWeb.Lib -s SGKWeb.CMS.UI

## 1. EF Core Database Provider

Entity Framework Core uses a provider model to access many different databases. There are different EF Core DB providers available for the different databases. These providers are available as NuGet packages. First, we need to install the NuGet package for the provider of the database we want to access.

| Database      | Provider Name                             | 
| ------------- | ----------------------------------------- |   
| SQL Server	| Microsoft.EntityFrameworkCore.SqlServer   | 
| MySQL 		| MySql.Data.EntityFrameworkCore            | 
| PostgreSQL	| Npgsql.EntityFrameworkCore.PostgreSQL     | 
| SQLite		| Microsoft.EntityFrameworkCore.SQLite      | 
| SQL Compact	| EntityFrameworkCore.SqlServerCompact40    |  
| In-memory	    | Microsoft.EntityFrameworkCore.InMemory    | 

``` batch
PM> Install-Package Microsoft.EntityFrameworkCore.SQLite
```

*Notice that the provider NuGet package may also install other dependent packages.*

## 2. EF Core Tools

Along with the DB provider package, you also need to install EF tools to execute EF Core commands.

**If you want to execute EF Core commands from Package Manager Console:**

``` batch
PM> Install-Package Microsoft.EntityFrameworkCore.Tools
```

**If you want to execute EF Core commands from .NET Core's CLI (Command Line Interface):**  
*The dotnet ef tool is no longer part of the .NET Core SDK. This change allows us to ship dotnet ef as a regular .NET CLI tool that can be installed as either a global or local tool. Run the following command to install dotnet ef tool:*

``` batch
> dotnet tool install --global dotnet-ef [--version 3.1.4]
```

The full list of commands can be accessed from within the command line by typing:
``` batch
> dotnet ef --help
``` 

### CODE-FIRST

#### EF-Core Code First: Creating the Model

Entity Framework needs to have a model (Entity Data Model) to communicate with the underlying database. It builds a model based on the shape of your domain classes, the Data Annotations and Fluent API configurations.

**1. Entity Classes**

An entity class or simply **entity** in Entity Framework is a class that maps to a database table. This class must be included as a `DbSet<TEntity>` type property in the DbContext class. EF API maps each entity to a table and each property of an entity to a column in the database.

An Entity can include two types of properties: Scalar Properties and Navigation Properties.

**Scalar Property**  
The primitive type properties are called scalar properties. Each scalar property maps to a column in the database table which stores an actual data.

**Navigation Property**  
The navigation property represents a relationship to another entity. There are two types of navigation properties: 
 + Reference Navigation 
 + Collection Navigation

*Reference Navigation Property*  
If an entity includes a property of another entity type, it is called a Reference Navigation Property. It points to a single entity and represents multiplicity of one (1) in the entity relationships. EF API will create a ForeignKey column in the table for the navigation properties that points to a PrimaryKey of another table in the database.

*Collection Navigation Property*  
If an entity includes a property of generic collection of an entity type, it is called a collection navigation property. It represents multiplicity of many (*). EF API does not create any column for the collection navigation property in the related table of an entity, but it creates a column in the table of an entity of generic collection. 

For this application; create a folder AppData\Entities and place the following entitiy classes in it. (By this way, you keep the project structure clean and organized)

``` csharp
public class Il
{
    [Key] // Required, since the key property name does not end with Id
    public string IlKodu { get; set; }
    public string IlAdi { get; set; }
}

public class Ilce
{
    [Key] // Required, since the key property name does not end with Id
    public string IlceKodu { get; set; }        
    public string IlceAdi { get; set; } 
    public Il Il { get; set; } // reference navigation property
}
``` 

**2. Create Context Class**

The context class represent a session with the underlying database using which you can perform CRUD (Create, Read, Update, Delete) operations. We create a context class by deriving the DbContext as follows.

``` csharp
public class AppDataContext : DbContext
{
    public AppDataContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("connection-string");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Il>().ToTable("Iller");
        modelBuilder.Entity<Ilce>().ToTable("Ilceler");            
    }
}
``` 

### CODE-FIRST

1) Install-Package Microsoft.EntityFrameworkCore.SQLite
2) Create entitiy classes and DbContext
3) Configure Services: `services.AddDbContext<CodeFirstDbContext>();`
2) Add Migration:
	PM> add-migration Mig0 -OutputDir AppLib/Migrations
	> dotnet ef migrations add Mig0 -o AppLib/Migrations
3) Update Database
	PM> update-database [-migration Mig0] [–verbose]
	> dotnet ef database update [Mig0] 


### DB-FIRST

1) Scaffold Db:
	PM> Scaffold-DbContext "Server=.\SQLExpress;Database=SchoolDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
	> dotnet ef dbcontext scaffold "Server=.\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models


> dotnet new console
> dotnet new sln
> dotnet sln add efcli.csproj

# Entity Framework

Entity Framework is an object-relational mapper (O/RM) that enables .NET developers to work with a database using .NET objects. It eliminates the need for most of the data-access code that developers usually need to write. 

The very first task of EF API is to build an Entity Data Model (EDM). EDM is an in-memory representation of the entire metadata: conceptual model, storage model, and mapping between them.

* Conceptual Model: The conceptual model contains the model classes and their relationships. This will be independent from your database table design.
* Storage Model: The storage model is the database design model which includes tables, views, stored procedures, and their relationships and keys.
* Mapping: Mapping consists of information about how the conceptual model is mapped to the storage model.

## Context Class

The context class is a most important class and it represent a session with the underlying database using which you can perform CRUD (Create, Read, Update, Delete) operations. The context class in Entity Framework derives from System.Data.Entity.DbContextDbContext.

```csharp
public class SchoolContext : DbContext
{
    public SchoolContext() { }
    // Entities        
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentAddress> StudentAddresses { get; set; }
    public DbSet<Grade> Grades { get; set; }
} 
```

## Entities

An entity in Entity Framework is a class that maps to a database table. This class must be included as a ```DbSet<TEntity>``` type property in the DbContext class. EF API maps each entity to a table and each property of an entity to a column in the database.

```csharp
public class Student {
    // scalar properties
    public int StudentID { get; set; }
    public string StudentName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public byte[]  Photo { get; set; }
    public decimal Height { get; set; }
    public float Weight { get; set; }
    
    //reference navigation property
    public Grade Grade { get; set; }
}

public class Grade {
    public int GradeId { get; set; }
    public string GradeName { get; set; }
    public string Section { get; set; }

    // collection navigation property
    public ICollection<Student> Students { get; set; }
}  
```

The above classes become entities when they are included as ```DbSet<TEntity>``` properties in a context class.

An Entity can include two types of properties: Scalar Properties and Navigation Properties.

### Scalar Property
The primitive type properties are called scalar properties. Each scalar property maps to a column in the database table which stores an actual data. For example, StudentID, StudentName, DateOfBirth, Photo, Height, Weight are the scalar properties in the Student entity class. EF API will create a column in the database table for each scalar property.

### Navigation Property
The navigation property represents a relationship to another entity. There are two types of navigation properties: 
* Reference Navigation 
* Collection Navigation

#### Reference Navigation Property

If an entity includes a property of another entity type, it is called a Reference Navigation Property. It points to a single entity and represents multiplicity of one (1) in the entity relationships.

EF API will create a ForeignKey column in the table for the navigation properties that points to a PrimaryKey of another table in the database.

#### Collection Navigation Property

If an entity includes a property of generic collection of an entity type, it is called a collection navigation property. It represents multiplicity of many (*).

EF API does not create any column for the collection navigation property in the related table of an entity, but it creates a column in the table of an entity of generic collection.

## Development Approaches with Entity Framework

There are three different approaches you can use while developing your application using Entity Framework:

* Database-First: Generate the context and entities for the existing database
* Code-First: Start writing your entities (domain classes) and context class first and then create the database from these classes using migration commands
* Model-First: you create entities, relationships, and inheritance hierarchies directly on the visual designer integrated in Visual Studio and then generate entities, the context class, and the database script from your visual model

## Persistence in Entity Framework

There are two scenarios when persisting (saving) an entity to the database using Entity Framework: 

* Connected Scenario: the same instance of the context class (derived from DbContext) is used in retrieving and saving entities (entity state tracking is automated)
* Disconnected Scenario: different instances of the context are used to retrieve and save entities to the database (entity state tracking is manual)

# EF Core

EF Core mainly targets the code-first approach and provides little support for the database-first approach. Entity Framework Core uses a provider model to access many different databases. EF Core includes providers as NuGet packages which you need to install.

EF Core DB provider (Database) | NuGet Package
--- | ---
SQL Server | Microsoft.EntityFrameworkCore.SqlServer
MySQL | MySql.Data.EntityFrameworkCore
PostgreSQL | Npgsql.EntityFrameworkCore.PostgreSQL
SQLite | Microsoft.EntityFrameworkCore.SQLite
SQL Compact | EntityFrameworkCore.SqlServerCompact40
In-memory | Microsoft.EntityFrameworkCore.InMemory

EF Core is not a part of .NET Core and standard .NET framework. It is available as a NuGet package. You need to install NuGet packages for the following two things to use EF Core in your application:

* EF Core DB provider
* EF Core tools

First, we need to install the NuGet package for the provider of the database we want to access. Here, we want to access SQL Server database, so we need to install Microsoft.EntityFrameworkCore.SqlServer NuGet package. 

```bash
PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer
```

Notice that the provider NuGet package also installed other dependent packages. 

Along with the DB provider package, you also need to install EF tools to execute EF Core commands. These make it easier to perform several EF Core-related tasks in your project at design time, such as migrations, scaffolding, etc.

In order to execute EF Core commands from Package Manager Console:

```bash
PM> Install-Package Microsoft.EntityFrameworkCore.Tools
```

If you want to execute EF Core commands from .NET Core's CLI (Command Line Interface):

```bash
PM> Install-Package Microsoft.EntityFrameworkCore.Tools.DotNet
```

## Entity Framework Core: DbContext

```csharp
public class SchoolContext : DbContext
{
    public SchoolContext() { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    
//entities
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
} 
```

The OnConfiguring() method allows us to select and configure the data source to be used with a context using DbContextOptionsBuilder. The OnModelCreating() method allows us to configure the model using ModelBuilder Fluent API.

## DB-First Approach: Creating a Model for an Existing Database

EF Core does not support visual designer for DB model and wizard to create the entity and context classes so we need to do reverse engineering using the Scaffold-DbContext command. This reverse engineering command creates entity and context classes (by deriving DbContext) based on the schema of the existing database.

```bash
PM> Scaffold-DbContext "Server=.\SQLExpress;Database=SchoolDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
```

Note: EF Core creates entity classes only for tables and not for StoredProcedures or Views.

If you use dotnet command line interface to execute EF Core commands then open command prompt and navigate to the root folder and execute the following dotnet ef dbcontext scaffold command:

```bash
> dotnet ef dbcontext scaffold "Server=.\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models
```

Note: Once you have created the model, you must use the Migration commands whenever you change the model to keep the database up to date with the model.

## Code-First Approach: Creating Database Schema using existing Context

We need to create entity classes and context classes first.

```csharp
public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; }
}

public class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; }
}

public class SchoolContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
        optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;");
    }
}
```

After creating the context and entity classes, it's time to add the migration to create a database.

```bash
PM> add-migration CreateSchoolDB
> dotnet ef migrations add CreateSchoolDB
```

This will create a new folder named Migrations in the project and create the ModelSnapshot files. After creating a migration, we still need to create the database using the update-database command

```bash
PM> update-database –verbose
> dotnet ef database update
```

This will create the database with the name and location specified in the connection string in the UseSqlServer() method. It creates a table for each DbSet property (Students and Courses).

This was the first migration to create a database. Now, whenever we add or update domain classes or configurations, we need to sync the database with the model using add-migration and update-database commands.

## Eager Loading

Entity Framework Core supports eager loading of related entities using the Include() extension method and projection query. In addition to this, it also provides the ThenInclude() extension method to load multiple levels of related entities.

```csharp
var studentWithGrade = context.Students
                        .Where(s => s.FirstName == "Bill")
                        .Include(s => s.Grade)
                        .FirstOrDefault();
```
The above query executes the following SQL query in the database.

```sql
SELECT TOP(1) [s].[StudentId], [s].[DoB], [s].[FirstName], [s].[GradeId],[s].[LastName], 
        [s].[MiddleName], [s.Grade].[GradeId], [s.Grade].[GradeName], [s.Grade].[Section]
FROM [Students] AS [s]
LEFT JOIN [Grades] AS [s.Grade] ON [s].[GradeId] = [s.Grade].[GradeId]
WHERE [s].[FirstName] = N'Bill'
```

We can also specify property name as a string in the Include() method

```csharp
var studentWithGrade = context.Students
                        .Where(s => s.FirstName == "Bill")
                        .Include("Grade")
                        .FirstOrDefault();
```

The example above is not recommended because it will throw a runtime exception if a property name is misspelled or does not exist. Always use the Include() method with a lambda expression, so that the error can be detected during compile time.

The Include() extension method can also be used after the FromSql() method, as shown below.

```csharp
var studentWithGrade = context.Students
                        .FromSql("Select * from Students where FirstName ='Bill'")
                        .Include(s => s.Grade)
                        .FirstOrDefault(); 
```

Note: The Include() extension method cannot be used after the DbSet.Find() method.

Use the Include() method multiple times to load multiple navigation properties of the same entity.

```csharp
var studentWithGrade = context.Students.Where(s => s.FirstName == "Bill")
                        .Include(s => s.Grade)
                        .Include(s => s.StudentCourses)
                        .FirstOrDefault();
```

The above query will execute two SQL queries in a single database round trip.

```sql
SELECT TOP(1) [s].[StudentId], [s].[DoB], [s].[FirstName], [s].[GradeId], [s].[LastName], 
        [s].[MiddleName], [s.Grade].[GradeId], [s.Grade].[GradeName], [s.Grade].[Section]
FROM [Students] AS [s]
LEFT JOIN [Grades] AS [s.Grade] ON [s].[GradeId] = [s.Grade].[GradeId]
WHERE [s].[FirstName] = N'Bill'
ORDER BY [s].[StudentId]
Go

SELECT [s.StudentCourses].[StudentId], [s.StudentCourses].[CourseId]
FROM [StudentCourses] AS [s.StudentCourses]
INNER JOIN (
    SELECT DISTINCT [t].*
    FROM (
        SELECT TOP(1) [s0].[StudentId]
        FROM [Students] AS [s0]
        LEFT JOIN [Grades] AS [s.Grade0] ON [s0].[GradeId] = [s.Grade0].[GradeId]
        WHERE [s0].[FirstName] = N'Bill'
        ORDER BY [s0].[StudentId]
    ) AS [t]
) AS [t0] ON [s.StudentCourses].[StudentId] = [t0].[StudentId]
ORDER BY [t0].[StudentId]
Go
```

EF Core introduced the new ThenInclude() extension method to load multiple levels of related entities.

```csharp
var student = context.Students.Where(s => s.FirstName == "Bill")
                .Include(s => s.Grade)
                .ThenInclude(g => g.Teachers)
                .FirstOrDefault();
```

In the above example, .Include(s => s.Grade) will load the Grade reference navigation property of the Student entity. .ThenInclude(g => g.Teachers) will load the Teacher collection property of the Grade entity. The ThenInclude method must be called after the Include method. The above will execute the following SQL queries in the database.

```sql
SELECT TOP(1) [s].[StudentId], [s].[DoB], [s].[FirstName], [s].[GradeId], [s].[LastName],
         [s].[MiddleName], [s.Grade].[GradeId], [s.Grade].[GradeName], [s.Grade].[Section]
FROM [Students] AS [s]
LEFT JOIN [Grades] AS [s.Grade] ON [s].[GradeId] = [s.Grade].[GradeId]
WHERE [s].[FirstName] = N'Bill'
ORDER BY [s.Grade].[GradeId]
Go

SELECT [s.Grade.Teachers].[TeacherId], [s.Grade.Teachers].[GradeId], [s.Grade.Teachers].[Name]
FROM [Teachers] AS [s.Grade.Teachers]
INNER JOIN (
    SELECT DISTINCT [t].*
    FROM (
        SELECT TOP(1) [s.Grade0].[GradeId]
        FROM [Students] AS [s0]
        LEFT JOIN [Grades] AS [s.Grade0] ON [s0].[GradeId] = [s.Grade0].[GradeId]
        WHERE [s0].[FirstName] = N'Bill'
        ORDER BY [s.Grade0].[GradeId]
    ) AS [t]
) AS [t0] ON [s.Grade.Teachers].[GradeId] = [t0].[GradeId]
ORDER BY [t0].[GradeId]
go
```

We can also load multiple related entities by using the projection query instead of Include() or ThenInclude() methods.

```csharp
var stud = context.Students.Where(s => s.FirstName == "Bill")
            .Select(s => new {
                Student = s,
                Grade = s.Grade,
                GradeTeachers = s.Grade.Teachers
            }).FirstOrDefault();
```

* Eager loading means that the related data is loaded from the database as part of the initial query.
* Explicit loading means that the related data is explicitly loaded from the database at a later time. Works the same way as in EF 6.
* Lazy loading means that the related data is transparently loaded from the database when the navigation property is accessed. Not supported in Entity Framework Core 2.0

# EFCore Conventions

Conventions are default rules using which Entity Framework builds a model based on your domain (entity) classes.

Schema  
EF Core will create all the database objects in the **dbo** schema by default.

Table  
EF Core will create database tables for all `DbSet<TEntity>` properties in a context class with the same name as the property. It will also create tables for entities which are not included as DbSet properties but are reachable through reference properties in other DbSet entities.

Column  
EF Core will create columns for all the scalar properties of an entity class with the same name as the property, by default. It uses the reference and collection properties in building relationships among corresponding tables in the database.

Column Data Type  
The data type for columns in the database table is depending on how the provider for the database has mapped C# data type to the data type of a selected database. The following table lists mapping between C# data type to SQL Server column data type.

C# Data Type | Mapping to SQL Server Data Type
--- | ---
int | int
string | nvarchar(Max)
decimal | decimal(18,2)
float | real
byte[] | varbinary(Max)
datetime | datetime
bool | bit
byte | tinyint
short | smallint
long | bigint
double | float
char | No mapping
sbyte | No mapping (throws exception)
object | No mapping

Nullable Column  
EF Core creates null columns for all reference data type and nullable primitive type properties e.g. string, `Nullable<int>`, decimal?.

NotNull Column  
EF Core creates NotNull columns in the database for all primary key properties, and primitive type properties e.g. int, float, decimal, DateTime etc..

Primary Key  
EF Core will create the primary key column for the property named Id or `<Entity Class Name>Id` (case insensitive). For example, EF Core will create a column as PrimaryKey in the Students table if the Student class includes a property named id, ID, iD, Id, studentid, StudentId, STUDENTID, or sTUdentID.

Foreign Key
As per the foreign key convention, EF Core API will create a foreign key column for each reference navigation property in an entity with one of the following naming patterns.

`<Reference Navigation Property Name>Id`  
`<Reference Navigation Property Name><Principal Primary Key Property Name>
`

In our example (Student and Grade entities), EF Core will create a foreign key column GradeId in the Students table.

Index  
EF Core creates a clustered index on Primarykey columns and a non-clustered index on ForeignKey columns, by default.

### One-to-Many Relationship Conventions in Entity Framework Core

There are certain conventions in Entity Framework which if followed in entity classes (domain classes) will automatically result in a one-to-many relationship between two tables in the database. You don't need to configure anything else.

#### Convention 1

We want to establish a one-to-many relationship between the Student and Grade entities where many students are associated with one Grade. It means that each Student entity points to a Grade. This can be achieved by including a reference navigation property of type Grade in the Student entity class, as shown below.

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    // reference navigation property
    public Grade Grade { get; set; }
}

public class Grade
{
    public int GradeId { get; set; }
    public string GradeName { get; set; }
    public string Section { get; set; }
}
```

In the above example, the Student class includes a reference navigation property of Grade class. So, there can be many students in a single grade. This will result in a one-to-many relationship between the Students and Grades table in the database, where the Students table includes foreign key Grade_GradeId. Notice that the reference property is nullable, so it creates a nullable foreign key column Grade_GradeId in the Students table.

#### Convention 2

Another convention is to include a collection navigation property in the principal entity as shown below.

```csharp
public class Student
{
    public int StudentId { get; set; }
    public string StudentName { get; set; }
}

public class Grade
{
    public int GradeId { get; set; }
    public string GradeName { get; set; }
    public string Section { get; set; }
    // collection navigation property
    public ICollection<Student> Students { get; set; } 
}
```

In the above example, the Grade entity includes a collection navigation property of type `ICollection<Student>`. This also results in a one-to-many relationship between the Student and Grade entities. This example produces the same result in the database as convention 1.

#### Convention 3

Including navigation properties at both ends will also result in a one-to-many relationship, as shown below.

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    // reference navigation property
    public Grade Grade { get; set; }
}

public class Grade
{
    public int GradeID { get; set; }
    public string GradeName { get; set; }
    public string Section { get; set; }
    // collection navigation property
    public ICollection<Student> Student { get; set; }
}
```
In the above example, the Student entity includes a reference navigation property of the Grade type and the Grade entity class includes a collection navigation property of the `ICollection<Student>` type which results in a one-to-many relationship. This example produces the same result in the database as convention 

#### Convention 4

A fully defined relationship at both ends will create a one-to-many relationship, as shown below.

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public int GradeId { get; set; }
    public Grade Grade { get; set; }
}

public class Grade
{

    public int GradeId { get; set; }
    public string GradeName { get; set; }
    
    public ICollection<Student> Student { get; set; }
}
```

In the above example, the Student entity includes foreign key property GradeId with its reference property Grade. This will create a one-to-many relationship with the NotNull foreign key column in the Students table. If the data type of GradeId is nullable integer, then it will create a null foreign key.

### One-to-One Relationship Conventions

In EF Core, a one-to-one relationship requires a reference navigation property at both sides. The following Student and StudentAddress entities follow the convention for the one-to-one relationship.

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
       
    public StudentAddress Address { get; set; }
}

public class StudentAddress
{
    public int StudentAddressId { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }
}
```
In the example above, the Student entity includes a reference navigation property of type StudentAddress and the StudentAddress entity includes a foreign key property StudentId and its corresponding reference property Student. This will result in a one-to-one relationship in corresponding tables Students and StudentAddresses in the database.

## Configurations in Entity Framework Core

Many times we want to customize the entity to table mapping and do not want to follow default conventions. EF Core allows us to configure domain classes in order to customize the EF model to database mappings. This programming pattern is referred to as Convention over Configuration.

There are two ways to configure domain classes in EF Core.

* By using Data Annotation Attributes
* By using Fluent API

#### Data Annotation Attributes

Data Annotations is a simple attribute based configuration method where different .NET attributes can be applied to domain classes and properties to configure the model.

Data annotation attributes are not dedicated to Entity Framework, as they are also used in ASP.NET MVC. This is why these attributes are included in separate namespace System.ComponentModel.DataAnnotations.

The following example demonstrates how the data annotations attributes can be applied to a domain class and properties to override conventions.

```csharp
[Table("StudentInfo")]
public class Student
{
    public Student() { }
        
    [Key]
    public int SID { get; set; }

    [Column("Name", TypeName="ntext")]
    [MaxLength(20)]
    public string StudentName { get; set; }

    [NotMapped]
    public int? Age { get; set; }
        
        
    public int StdId { get; set; }

    [ForeignKey("StdId")]
    public virtual Standard Standard { get; set; }
}
```
#### Fluent API

Another way to configure domain classes is by using Entity Framework Fluent API. Entity Framework Fluent API is used to configure domain classes to override conventions and it is based on a Fluent API design pattern (a.k.a Fluent Interface) where the result is formulated by method chaining.

Entity Framework Core Fluent API configures the following aspects of a model:

* Model Configuration: Configures an EF model to database mappings. Configures the default Schema, DB functions, additional data annotation attributes and entities to be excluded from mapping.
* Entity Configuration: Configures entity to table and relationships mapping e.g. PrimaryKey, AlternateKey, Index, table name, one-to-one, one-to-many, many-to-many relationships etc.
* Property Configuration: Configures property to column mapping e.g. column name, default value, nullability, Foreignkey, data type, concurrency column etc.

#### Fluent API Configurations

Override the OnModelCreating method and use a parameter modelBuilder of type ModelBuilder to configure domain classes, as shown below.

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    //Property Configurations
    modelBuilder.Entity<Student>().Property(s => s.StudentId)
            .HasColumnName("Id")
            .HasDefaultValue(0)
            .IsRequired();
}
```
**Note:** Fluent API configurations have higher precedence than data annotation attributes.

### Configure One-to-Many Relationships using Fluent API

Consider the following Student and Grade classes where the Grade entity includes many Student entities.

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int CurrentGradeId { get; set; }
    public Grade Grade { get; set; }
}

public class Grade
{
    public int GradeId { get; set; }
    public string GradeName { get; set; }
    public string Section { get; set; }

    public ICollection<Student> Students { get; set; }
}
```

Configure the one-to-many relationship for the above entities using Fluent API by overriding the OnModelCreating method in the context class, as shown below.

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Student>()
        .HasOne<Grade>(s => s.Grade)
        .WithMany(g => g.Students)
        .HasForeignKey(s => s.CurrentGradeId)
        .OnDelete(DeleteBehavior.Cascade);;
}
```
* Cascade : Dependent entities will be deleted when the principal entity is deleted.
* ClientSetNull: The values of foreign key properties in the dependent entities will be set to null.
* Restrict: Prevents Cascade delete.
* SetNull: The values of foreign key properties in the dependent entities will be set to null.

### Configure One-to-One Relationships using Fluent API

Let's configure a one-to-one relationship between the following Student and StudentAddress entities, which do not follow the foreign key convention.

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
       
    public StudentAddress Address { get; set; }
}

public class StudentAddress
{
    public int StudentAddressId { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }

    public int AddressOfStudentId { get; set; }
    public Student Student { get; set; }
}
```

To configure a one-to-one relationship using Fluent API in EF Core, use the HasOne, WithOne and HasForeignKey methods, as shown below.

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Student>()
        .HasOne<StudentAddress>(s => s.Address)
        .WithOne(ad => ad.Student)
        .HasForeignKey<StudentAddress>(ad => ad.AddressOfStudentId);
}
```

### Configure Many-to-Many Relationships using Fluent API

Let's implement a many-to-many relationship between the following Student and Course entities, where one student can enroll for many courses and, in the same way, one course can be joined by many students.

```csharp
public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; }
}

public class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public string Description { get; set; }
}
```

The many-to-many relationship in the database is represented by a joining table which includes the foreign keys of both tables. Also, these foreign keys are composite primary keys.

Students (StudentId, Name) <-- StudentSourses (StudentId, CourseId) --> Courses (CourseId, Name)

There are no default conventions available in Entity Framework Core which automatically configure a many-to-many relationship. You must configure it using Fluent API.

In the Entity Framework 6.x or prior, EF API used to create the joining table for many-to-many relationships. We need not to create a joining entity for a joining table (however, we can of course create a joining entity explicitly in EF 6).

In Entity Framework Core, this has not been implemented yet. We must create a joining entity class for a joining table. The joining entity for the above Student and Course entities should include a foreign key property and a reference navigation property for each entity.

```csharp
public class StudentCourse
{
    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }
}

public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; }

    public IList<StudentCourse> StudentCourses { get; set; }
}

public class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public string Description { get; set; }

    public IList<StudentCourse> StudentCourses { get; set; }
}
```

As you can see above, the Student and Course entities now include a collection navigation property of StudentCourse type. The StudentCourse entity already includes the foreign key property and navigation property for both, Student and Course. This makes it a fully defined one-to-many relationship between Student & StudentCourse and Course & StudentCourse.

Now, the foreign keys must be the composite primary key in the joining table. This can only be configured using Fluent API, as below.

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<StudentCourse>().HasKey(sc => new { sc.StudentId, sc.CourseId });
}
```

This is how you can configure many-to-many relationships if entities follow the conventions for one-to-many relationships with the joining entity. Suppose that the foreign key property names do not follow the convention (e.g. SID instead of StudentId and CID instead of CourseId), then you can configure it using Fluent API, as shown below.

```csharp
modelBuilder.Entity<StudentCourse>().HasKey(sc => new { sc.SId, sc.CId });

modelBuilder.Entity<StudentCourse>()
    .HasOne<Student>(sc => sc.Student)
    .WithMany(s => s.StudentCourses)
    .HasForeignKey(sc => sc.SId);


modelBuilder.Entity<StudentCourse>()
    .HasOne<Course>(sc => sc.Course)
    .WithMany(s => s.StudentCourses)
    .HasForeignKey(sc => sc.CId);
```

## Shadow Property in Entity Framework Core

Shadow properties are the properties that are not defined in your .NET entity class directly; instead, you configure it for the particular entity type in the entity data model. They can be configured in the OnModelCreating() method of the context class.

Consider the following Student entity class.

```csharp
public class Student
{
    public int StudentID { get; set; }
    public string StudentName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public decimal Height { get; set; }
    public float Weight { get; set; }
}
```

The above Student class does not include CreatedDate and UpdatedDate properties to maintain created or updated time. We will configure them as shadow properties on the Student entity.

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<Student>().Property<DateTime>("CreatedDate");
    modelBuilder.Entity<Student>().Property<DateTime>("UpdatedDate");
}
```

Once we define shadow properties, we need to update the database schema because shadow properties will be mapped to the corresponding database column.

```bash
PM> add-migration <migration-name>
PM> update-database
```

### Access Shadow Property
You can get or set the values of the shadow properties using the Property() method of EntityEntry. The following code access the value of the shadow property.

```csharp
using (var context = new SchoolContext())
{
    var std = new Student(){ StudentName = "Bill"  };
    
    // sets the value to the shadow property
    context.Entry(std).Property("CreatedDate").CurrentValue = DateTime.Now;

    // gets the value of the shadow property
    var createdDate = context.Entry(std).Property("CreatedDate").CurrentValue; 
}
```

However, in our scenario, we want to set the value to these shadow properties automatically on the SaveChanges() method, so that we don't have to set them manually on each entity object. So, override the SaveChanges() method in the context class, as shown below.

```csharp
public override int SaveChanges()
{
    var entries = ChangeTracker
        .Entries()
        .Where(e =>
                e.State == EntityState.Added
                || e.State == EntityState.Modified);

    foreach (var entityEntry in entries)
    {
        entityEntry.Property("UpdatedDate").CurrentValue = DateTime.Now;

        if (entityEntry.State == EntityState.Added)
        {
            entityEntry.Property("CreatedDate").CurrentValue = DateTime.Now;
        }
    }

    return base.SaveChanges();
}
```
This will automatically set values to CreatedDate and UpdatedDate shadow properties.

### Configuring Shadow Properties on All Entities
You can configure shadow properties on all entities at once, rather than configuring them manually for all. For example, we can configure CreatedDate and UpdatedDate on all the entities at once, as shown below.

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    var allEntities = modelBuilder.Model.GetEntityTypes();

    foreach (var entity in allEntities)
    {
        entity.AddProperty("CreatedDate",typeof(DateTime));
        entity.AddProperty("UpdatedDate",typeof(DateTime));
    }
}
```

## Execute Raw SQL Queries in Entity Framework Core
Entity Framework Core provides the DbSet.FromSql() method to execute raw SQL queries for the underlying database and get the results as entity objects. The following example demonstrates executing a raw SQL query to MS SQL Server database.

```csharp
var context = new SchoolContext();

var students = context.Students
                  .FromSql("Select * from Students where Name = 'Bill'")
                  .ToList();
```
The FromSql method allows parameterized queries using string interpolation syntax in C#, as shown below.

```csharp
string name = "Bill";

var context = new SchoolContext();
var students = context.Students
                    .FromSql($"Select * from Students where Name = '{name}'")
                    // or
                    .FromSql("Select * from Students where Name = '{0}'", name)
                    .ToList();
```
The examples above will execute the following SQL query to the SQL Server database:

```sql
exec sp_executesql N'Select * from Students where Name = ''@p0''
',N'@p0 nvarchar(4000)',@p0=N'Bill'
go
```
**FromSql Limitations**  
* SQL queries must return entities of the same type as `DbSet<T>` type. e.g. the specified query cannot return the Course entities if FromSql is used after Students. Returning ad-hoc types from FromSql() method is in the backlog.
* The SQL query must return all the columns of the table. e.g. context.Students.FromSql("Select StudentId, LastName from Students).ToList() will throw an exception.
* The SQL query cannot include JOIN queries to get related data. Use Include method to load related entities after FromSql() method.

## Working with Stored Procedure in Entity Framework Core
EF Core provides the following methods to execute a stored procedure:

* `DbSet<TEntity>`.FromSql()
* DbContext.Database.ExecuteSqlCommand()

There are some limitations on the execution of database stored procedures using FromSql or ExecuteSqlCommand methods in EF Core2:

* Result must be an entity type. This means that a stored procedure must return all the columns of the corresponding table of an entity.
* Result cannot contain related data. This means that a stored procedure cannot perform JOINs to formulate the result.
* Insert, Update and Delete procedures cannot be mapped with the entity, so the SaveChanges method cannot call stored procedures for CUD operations.

## Logging in Entity Framework Core

We often need to log the SQL and change tracking information for debugging purposes in EF Core. EF Core logging automatically integrates with the logging mechanisms of .NET Core.

Entity Framework Core integrates with the .NET Core logging to log SQL and change tracking information to the various output targets. First, install the Nuget package for logging provider of your choice and then tie up the DbContext to ILoggerFactory.

Let's install the logging provider's NuGet package. Here, we will display the logs on the console, so install the Microsoft.Extensions.Logging.Console NuGet package from the NuGet Package Manager or execute the following command in the Package Manager Console:

```bash
PM> Install-Package Microsoft.Extensions.Logging.Console
```

After installing the console logger provider, you need to create a static/singleton instance of the LoggerFactory and then tie it with a DbContext, as shown below.

```csharp
public class SchoolContext : DbContext
{
    //static LoggerFactory object
    public static readonly ILoggerFactory loggerFactory = new LoggerFactory(new[] {
              new ConsoleLoggerProvider((_, __) => true, true)
        });
    //or
    // public static readonly ILoggerFactory loggerFactory  = new LoggerFactory().AddConsole((_,___) => true);
    
    public SchoolContext():base() { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(loggerFactory)  //tie-up DbContext with LoggerFactory object
            .EnableSensitiveDataLogging()  
            .UseSqlServer(@"Server=.\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;");
    }
        
    public DbSet<Student> Students { get; set; }
}
```

In the above example, we have created an object of the LoggerFactory class and assigned it to the ILoggerFactory type static variable. Then, we passed this object in the optionsBuilder.UseLoggerFactory() method in the OnConfiguring() method. This will enable the DbContext to share the information with the loggerFactory object, which in turn will display all the logging information on the console.

By default, EF Core will not log sensitive data, such as filter parameter values. So, call the EnableSensitiveDataLogging() to log sensitive data.

Filter Logs
In the above configuration, the DbContext logged all the information while saving an entity. Sometime you don't want to log all the information and filter some unwanted logs. In EF Core, you can filter logs by specifying the logger category and log level.

EF Core 2.x includes the DbLoggerCategory class to get an Entity Framework Core logger categories using its Name property. To log only SQL queries, specify the DbLoggerCategory.Database.Command category and LogLevel.Information in the lambda expression in the constructor of the ConsoleLoggerProvider, as shown below.

```csharp
public static readonly ILoggerFactory consoleLoggerFactory  
            = new LoggerFactory(new[] {
                  new ConsoleLoggerProvider((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name &&
                    level == LogLevel.Information, true)
                });
```

Or, just call the AddConsole() method on LoggerFactory to log SQL queries, by default.

```csharp
public static readonly ILoggerFactory consoleLoggerFactory
         = new LoggerFactory().AddConsole();
```

### Migration in Entity Framework Core
Migration is a way to keep the database schema in sync with the EF Core model by preserving data. 

EF Core API builds the EF Core model from the domain (entity) classes and EF Core migrations will create or update the database schema based on the EF Core model. Whenever you change the domain classes, you need to run migration to keep the database schema up to date.

EF Core migrations are a set of commands which you can execute in NuGet Package Manager Console or in dotnet Command Line Interface (CLI).

PMC Command | dotnet CLI command | Usage
--- | --- | ---
`add-migration <migration name>` | `Add <migration name>` | Creates a migration by adding a migration snapshot.
Remove-migration | Remove | Removes the last migration snapshot.
Update-database | Update | Updates the database schema based on the last migration snapshot.
Script-migration | Script | Generates a SQL script using all the migration snapshots.

# Other Document

## EF-Core Conventions

**Schema:** EF Core will create all the database objects in the dbo schema by default.

**Table:** EF Core will create database tables for all `DbSet<TEntity>` properties in a context class with the same name as the property. It will also create tables for entities which are not included as DbSet properties but are reachable through reference properties in other DbSet entities.

**Column:** EF Core will create columns for all the scalar properties of an entity class with the same name as the property, by default. It uses the reference and collection properties in building relationships among corresponding tables in the database.

**Nullable Column:** EF Core creates null columns for all reference data type and nullable primitive type properties e.g. string, `Nullable<int>`, decimal?.

**NotNull Column:** EF Core creates NotNull columns in the database for all primary key properties, and primitive type properties e.g. int, float, decimal, DateTime etc..

**Primary Key:** EF Core will create the primary key column for the property named Id or `<Entity Class Name>Id` (case insensitive).

**Foreign Key:** As per the foreign key convention, EF Core API will create a foreign key column for each reference navigation property in an entity with one of the following naming patterns.

 + `<Reference Navigation Property Name>Id`  
 + `<Reference Navigation Property Name><Principal Primary Key Property Name>`

**Index:** EF Core creates a clustered index on Primarykey columns and a non-clustered index on ForeignKey columns, by default.

### One-to-Many Relationship Conventions

Consider the followinf entity classes:

``` csharp
public class Student {
    public int StudentId { get; set; }
    public string StudentName { get; set; }
}
       
public class Grade {
    public int GradeId { get; set; }
    public string GradeName { get; set; }
    public string Section { get; set; }
}
``` 

#### Convention 1
We want to establish a one-to-many relationship where many students are associated with one grade. This can be achieved by including a reference navigation property in the dependent entity as shown below. (here, the Student entity is the dependent entity and the Grade entity is the principal entity).

``` csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    // reference navigation property
    public Grade Grade { get; set; }
}

public class Grade
{
    public int GradeId { get; set; }
    public string GradeName { get; set; }
    public string Section { get; set; }
}
```
In the example above, the Student entity class includes a reference navigation property of Grade type. This allows us to link the same Grade to many different Student entities, which creates a one-to-many relationship between them. This will produce a one-to-many relationship between the Students and Grades tables in the database, where Students table includes a nullable foreign key GradeId, as shown below. EF Core will create a shadow property for the foreign key named GradeId in the conceptual model, which will be mapped to the GradeId foreign key column in the Students table.

#### Convention 2
Another convention is to include a collection navigation property in the principal entity as shown below.

``` csharp
public class Student
{
    public int StudentId { get; set; }
    public string StudentName { get; set; }
}

public class Grade
{
    public int GradeId { get; set; }
    public string GradeName { get; set; }
    public string Section { get; set; }
    // collection navigation property
    public ICollection<Student> Students { get; set; } 
}
```
In the example above, the Grade entity includes a collection navigation property of type `ICollection<student>`. This will allow us to add multiple Student entities to a Grade entity, which results in a one-to-many relationship between Students and Grades tables in the database, same as in convention 1.

#### Convention 3
Another EF convention for the one-to-many relationship is to include navigation property at both ends, which will also result in a one-to-many relationship (convention 1 + convention 2).

``` csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    // reference navigation property
    public Grade Grade { get; set; }
}

public class Grade
{
    public int GradeID { get; set; }
    public string GradeName { get; set; }
    // collection navigation property
    public ICollection<Student> Students { get; set; }
}
```
In the example above, the Student entity includes a reference navigation property of Grade type and the Grade entity class includes a collection navigation property `ICollection<Student>`, which results in a one-to-many relationship between corresponding database tables Students and Grades, same as in convention 1.

#### Convention 4
Defining the relationship fully at both ends with the foreign key property in the dependent entity creates a one-to-many relationship.

``` csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int GradeId { get; set; } // foreign key property
    public Grade Grade { get; set; } // reference navigation property
}

public class Grade
{
    public int GradeId { get; set; }
    public string GradeName { get; set; }

    public ICollection<Student> Students { get; set; } // collection navigation property
}
```
In the above example, the Student entity includes a foreign key property GradeId of type int and its reference navigation property Grade. At the other end, the Grade entity also includes a collection navigation property `ICollection<Student>`. This will create a one-to-many relationship with the NotNull foreign key column in the Students table.

These are the conventions which automatically create a one-to-many relationship in the corresponding database tables. If entities do not follow the above conventions, then you can use Fluent API to configure the one-to-many relationship.

### One-to-One Relationship Conventions

In EF Core, a one-to-one relationship requires a reference navigation property at both sides. The following Student and StudentAddress entities follow the convention for the one-to-one relationship.

``` csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
       
    public StudentAddress Address { get; set; }
}

public class StudentAddress
{
    public int StudentAddressId { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }
}
```

In the example above, the Student entity includes a reference navigation property of type StudentAddress and the StudentAddress entity includes a foreign key property StudentId and its corresponding reference property Student. This will result in a one-to-one relationship in corresponding tables Students and StudentAddresses in the database.

Use Fluent API to configure one-to-one relationships if entities do not follow the conventions.

## Configurations in Entity Framework Core

There are two ways to configure domain classes in EF Core. 

 + By using Data Annotation Attributes
 + By using Fluent API

**Data Annotation Attributes**  
Data Annotations is a simple attribute based configuration method where different .NET attributes can be applied to domain classes and properties to configure the model.

Data annotation attributes are not dedicated to Entity Framework, as they are also used in ASP.NET MVC. This is why these attributes are included in separate namespace System.ComponentModel.DataAnnotations.

The following example demonstrates how the data annotations attributes can be applied to a domain class and properties to override conventions.

``` csharp
[Table("StudentInfo")]
public class Student
{
    public Student() { }
        
    [Key]
    public int SID { get; set; }

    [Column("Name", TypeName="ntext")]
    [MaxLength(20)]
    public string StudentName { get; set; }

    [NotMapped]
    public int? Age { get; set; }
        
        
    public int StdId { get; set; }

    [ForeignKey("StdId")]
    public virtual Standard Standard { get; set; }
}
```

Another way to configure domain classes is by using Entity Framework Fluent API. EF Fluent API is based on a Fluent API design pattern (a.k.a Fluent Interface) where the result is formulated by method chaining.

### Fluent API in Entity Framework Core

Entity Framework Fluent API is used to configure domain classes to override conventions. EF Fluent API is based on a Fluent API design pattern (a.k.a Fluent Interface) where the result is formulated by method chaining.

In Entity Framework Core, the ModelBuilder class acts as a Fluent API. By using it, we can configure many different things, as it provides more configuration options than data annotation attributes.

Entity Framework Core Fluent API configures the following aspects of a model:

 + Model Configuration: Configures an EF model to database mappings. Configures the default Schema, DB functions, additional data annotation attributes and entities to be excluded from mapping.
 + Entity Configuration: Configures entity to table and relationships mapping e.g. PrimaryKey, AlternateKey, Index, table name, one-to-one, one-to-many, many-to-many relationships etc.
 + Property Configuration: Configures property to column mapping e.g. column name, default value, nullability, Foreignkey, data type, concurrency column etc.

**Fluent API Configurations**  
Override the OnModelCreating method and use a parameter modelBuilder of type ModelBuilder to configure domain classes, as shown below.

``` csharp
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Write Fluent API configurations here

        //Property Configurations

        // either: Fluent API method chained calls
        modelBuilder.Entity<Student>()
                .Property(s => s.StudentId)
                .HasColumnName("Id")
                .HasDefaultValue(0)
                .IsRequired();

        // or: Separate method calls
        modelBuilder.Entity<Student>().Property(s => s.StudentId).HasColumnName("Id");
        modelBuilder.Entity<Student>().Property(s => s.StudentId).HasDefaultValue(0);
        modelBuilder.Entity<Student>().Property(s => s.StudentId).IsRequired();
    }
``` 

#### Configure One-to-Many Relationships using Fluent API
Configure the one-to-many relationship for the above entities using Fluent API by overriding the OnModelCreating method in the context class, as shown below.

``` csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Student>()
        .HasOne<Grade>(s => s.Grade)
        .WithMany(g => g.Students)
        .HasForeignKey(s => s.CurrentGradeId);
}
``` 
Let's understand the above code step by step.

 + First, we need to start configuring with one entity class, either Student or Grade. So, `modelBuilder.Entity<student>()` starts with the Student entity.
 + Then, `.HasOne<Grade>(s => s.Grade)` specifies that the Student entity includes a Grade type property named Grade.
 + Now, we need to configure the other end of the relationship, the Grade entity. The `.WithMany(g => g.Students)` specifies that the Grade entity class includes many Student entities. Here, WithMany infers collection navigation property.
 + The `.HasForeignKey<int>(s => s.CurrentGradeId);` specifies the name of the foreign key property CurrentGradeId. This is optional. Use it only when you have the foreign key Id property in the dependent class.

Alternatively, you can start configuring the relationship with the Grade entity instead of the Student entity, as shown below.

``` csharp
modelBuilder.Entity<Grade>()
    .HasMany<Student>(g => g.Students)
    .WithOne(s => s.Grade)
    .HasForeignKey(s => s.CurrentGradeId);
``` 

**Configure Cascade Delete using Fluent API**  
Cascade delete automatically deletes the child row when the related parent row is deleted. For example, if a Grade is deleted, then all the Students in that grade should also be deleted from the database automatically.

Use the OnDelete method to configure the cascade delete between Student and Grade entities, as shown below.

``` csharp
modelBuilder.Entity<Grade>()
    .HasMany<Student>(g => g.Students)
    .WithOne(s => s.Grade)
    .HasForeignKey(s => s.CurrentGradeId)
    .OnDelete(DeleteBehavior.Cascade);
``` 

 + Cascade : Dependent entities will be deleted when the principal entity is deleted.
 + ClientSetNull: The values of foreign key properties in the dependent entities will be set to null.
 + Restrict: Prevents Cascade delete.
 + SetNull: The values of foreign key properties in the dependent entities will be set to null.

#### Configure One-to-One Relationships using Fluent API
Let's configure a one-to-one relationship between the following Student and StudentAddress entities, which do not follow the foreign key convention.

``` csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Student>()
        .HasOne<StudentAddress>(s => s.Address)
        .WithOne(ad => ad.Student)
        .HasForeignKey<StudentAddress>(ad => ad.AddressOfStudentId);
}
``` 

You can start configuring with the StudentAddress entity in the same way, as below.

``` csharp
modelBuilder.Entity<StudentAddress>()
    .HasOne<Student>(ad => ad.Student)
    .WithOne(s => s.Address)
    .HasForeignKey<StudentAddress>(ad => ad.AddressOfStudentId);
```

### Configure Many-to-Many Relationships in Entity Framework Core
There are no default conventions available in Entity Framework Core which automatically configure a many-to-many relationship. You must configure it using Fluent API.

In the Entity Framework 6.x or prior, EF API used to create the joining table for many-to-many relationships. We need not to create a joining entity for a joining table (however, we can of course create a joining entity explicitly in EF 6).

In Entity Framework Core, this has not been implemented yet. We must create a joining entity class for a joining table. The joining entity for the above Student and Course entities should include a foreign key property and a reference navigation property for each entity.

The steps for configuring many-to-many relationships would the following:

 + Define a new joining entity class which includes the foreign key property and the reference navigation property for each entity.
 + Define a one-to-many relationship between other two entities and the joining entity, by including a collection navigation property in entities at both sides (Student and Course, in this case).
 + Configure both the foreign keys in the joining entity as a composite key using Fluent API.

To create a many-to-many relation for the following entities: 

``` csharp
public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; }

    public IList<StudentCourse> StudentCourses { get; set; }
}

public class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public string Description { get; set; }

    public IList<StudentCourse> StudentCourses { get; set; }
}
``` 

The joining entity StudentCourse will be:

``` csharp
public class StudentCourse
{
    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }
}
``` 

As you can see above, the Student and Course entities now include a collection navigation property of StudentCourse type. The StudentCourse entity already includes the foreign key property and navigation property for both, Student and Course. This makes it a fully defined one-to-many relationship between Student & StudentCourse and Course & StudentCourse.

Now, the foreign keys must be the composite primary key in the joining table. This can only be configured using Fluent API, as below.

``` csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<StudentCourse>().HasKey(sc => new { sc.StudentId, sc.CourseId });
}
``` 

In the above code, `modelBuilder.Entity<StudentCourse>().HasKey(sc => new { sc.StudentId, sc.CourseId })` configures StudentId and CourseId as the composite key.

This is how you can configure many-to-many relationships if entities follow the conventions for one-to-many relationships with the joining entity. Suppose that the foreign key property names do not follow the convention (e.g. SID instead of StudentId and CID instead of CourseId), then you can configure it using Fluent API, as shown below.

``` csharp
modelBuilder.Entity<StudentCourse>().HasKey(sc => new { sc.SId, sc.CId });

modelBuilder.Entity<StudentCourse>()
    .HasOne<Student>(sc => sc.Student)
    .WithMany(s => s.StudentCourses)
    .HasForeignKey(sc => sc.SId);


modelBuilder.Entity<StudentCourse>()
    .HasOne<Course>(sc => sc.Course)
    .WithMany(s => s.StudentCourses)
    .HasForeignKey(sc => sc.CId);
``` 

**Note:** EF team will include a feature where we don't need to create a joining entity for many-to-many relationships in future.

## Execute Raw SQL Queries in Entity Framework Core

Entity Framework Core provides the DbSet.FromSql() method to execute raw SQL queries for the underlying database and get the results as entity objects. The following example demonstrates executing a raw SQL query to MS SQL Server database.

``` csharp
var context = new SchoolContext();

var students = context.Students
                  .FromSql("Select * from Students where Name = 'Bill'")
                  .ToList();
``` 

**Parameterized Query**  
The FromSql method allows parameterized queries using string interpolation syntax in C#, as shown below.

``` csharp
string name = "Bill";
var context = new SchoolContext();

var students = context.Students
                    .FromSql($"Select * from Students where Name = '{name}'")
                    .ToList();

// The following is also valid.

var students = context.Students
                    .FromSql("Select * from Students where Name = '{0}'", name)
                    .ToList();
``` 

You can also use LINQ Operators after a raw query using FromSql method.

``` csharp
var students = context.Students
                    .FromSql("Select * from Students where Name = '{0}'", name)
                    .OrderBy(s => s.StudentId)
                    .ToList();
``` 

FromSql Limitations
 + SQL queries must return entities of the same type as `DbSet<T>` type. e.g. the specified query cannot return the Course entities if FromSql is used after Students. Returning ad-hoc types from FromSql() method is in the backlog.
 + The SQL query must return all the columns of the table. e.g. context.Students.FromSql("Select StudentId, LastName from Students).ToList() will throw an exception.
 + The SQL query cannot include JOIN queries to get related data. Use Include method to load related entities after FromSql() method.

## Working with Stored Procedure in Entity Framework Core

EF Core provides the following methods to execute a stored procedure:

 + `DbSet<TEntity>.FromSql()`
 + `DbContext.Database.ExecuteSqlCommand()`

There are some limitations on the execution of database stored procedures using FromSql or ExecuteSqlCommand methods

 + Result must be an entity type. This means that a stored procedure must return all the columns of the corresponding table of an entity.
 + Result cannot contain related data. This means that a stored procedure cannot perform JOINs to formulate the result.
 + Insert, Update and Delete procedures cannot be mapped with the entity, so the SaveChanges method cannot call stored procedures for CUD operations.

In the database, we can execute our sample GetStudents stored procedure with an INPUT parameter value like below:

``` sql
GetStudents "Bill"
-- or
exec GetStudents "Bill"
``` 

#### Execute Stored Procedures using FromSql

You can execute SP using FromSql method in EF Core in the same way as above, as shown below.

``` csharp
var context = new SchoolContext(); 

var students = context.Students.FromSql("GetStudents 'Bill'").ToList();

// or
var name = "Bill";
var students = context.Students
                      .FromSql($"GetStudents {name}")
                      .ToList();

// or
var param = new SqlParameter("@FirstName", "Bill"); // way1

var param = new SqlParameter() { // way2
                    ParameterName = "@FirstName",
                    SqlDbType =  System.Data.SqlDbType.VarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Size = 50,
                    Value = "Bill"
};

var students = context.Students.FromSql("GetStudents @FirstName", param).ToList();

// or
// You can also specify @p0 for the first parameter, @p1 for the second, and so on.
var students = context.Students.FromSql("GetStudents @p0","Bill").ToList();
``` 

In the above example, @p0 is used for the first parameter because named parameters are not supported yet in EF Core.

#### Execute Stored Procedure using ExecuteSqlCommand()

``` csharp
var context = new SchoolContext(); 

var rowsAffected = context.Database.ExecuteSqlCommand("Update Students set FirstName = 'Bill' where StudentId = 1;");
context.Database.ExecuteSqlCommand("CreateStudents @p0, @p1", parameters: new[] { "Bill", "Gates" });
``` 

In the same way, you can execute stored procedures for Update and Delete commands.

# Dependencies

``` bash
PM> Install-Package Newtonsoft.Json
PM> Install-Package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore

Install-Package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
``` 



``` sql
DECLARE @json NVARCHAR(MAX);
SET @json = N'[
  {
    "ilkodu": "01",
    "iladi": "ADANA"
  },
  {
    "ilkodu": "02",
    "iladi": "ADIYAMAN"
  }
]';
select * into App101.dbo.my_iller from (
SELECT *
FROM OPENJSON(@json)
WITH (
    ilkodu NVARCHAR(2) 'strict $.ilkodu', -- yada (strict neye yarıyor bak)
    ilkodu NVARCHAR(2) '$.ilkodu',
    iladi NVARCHAR(150) '$.iladi'
)) as T;




select 
ilkodu, iladi, ilcekodu, ilceadi, 
semtkodu, ROW_NUMBER() OVER(PARTITION BY ilkodu, ilcekodu ORDER BY ilkodu, ilcekodu ASC) AS Row#,
semtadi 
from App101.dbo.my_sbb;


select 
ilkodu, iladi, ilcekodu, ilceadi, 
concat(ilkodu,ilcekodu,FORMAT(RowNum, '0000')) as semtkodu,
semtadi 
into App101.dbo.my_sbb2 from(
select 
ilkodu, iladi, ilcekodu, ilceadi, 
semtkodu, ROW_NUMBER() OVER(PARTITION BY ilkodu, ilcekodu ORDER BY ilkodu, ilcekodu ASC) AS RowNum,
semtadi 
from App101.dbo.my_sbb) as T;


SELECT [IlKodu]
      ,[il]	  
      ,[ilçe]
	  ,DENSE_RANK() OVER (ORDER BY IlKodu, ilçe) AS IlceKodu
      ,[semt_bucak_belde]
      ,[Mahalle]
      ,[PK]
      ,[Pkey]
  FROM [App101].[dbo].[RawData2]
  order by IlKodu, ilçe

-- concat(IlKodu, format( DENSE_RANK() OVER (ORDER BY IlKodu, ilçe), '000')) AS IlceKodu

select top 1 * from App101.dbo.IL_ILCE_SBB_MAHALLE for json auto
select top 1 * from App101.dbo.IL_ILCE_SBB_MAHALLE for json path, root('rootname')

https://www.sqlshack.com/importexport-json-data-using-sql-server-2016/
https://docs.microsoft.com/en-us/sql/relational-databases/json/format-query-results-as-json-with-for-json-sql-server?view=sql-server-ver15
https://docs.microsoft.com/en-us/sql/relational-databases/json/json-data-sql-server?view=sql-server-ver15







DECLARE @SearchWord NVARCHAR(30)  
SET @SearchWord = N'HÜ'  
SELECT [IlKod]
      ,[Il]
      ,[IlceKod]
      ,[Ilce]
      ,[SbbKod]
      ,[SemtBucakBelde]
      ,[MahalleKod]
      ,[Mahalle]
      ,[PostaKod]
  FROM [App101].[dbo].[IL_ILCE_SBB_MAHALLE]
  where CONTAINS(Mahalle, @SearchWord); 


-- Cannot use a CONTAINS or FREETEXT predicate on table or indexed view 'App101.dbo.IL_ILCE_SBB_MAHALLE' because it is not full-text indexed.

  -- 1) Make sure you have full-text search feature installed.
  -- 2) Create full-text search catalog (if needed)

--First check if any catalog already exists

  select * from sys.fulltext_catalogs 
  --If no catalog is found create one

  use [DatabaseName]
  create fulltext catalog FullTextCatalog as default
--you can verify that the catalog was created in the same way as above

-- Create full-text search index.

  create fulltext index on Production.ProductDescription(Description) key index PK_ProductDescription_ProductDescriptionID

-- Before you create the index, make sure:
-- you don't already have full-text search index on the table as only one full-text search index allowed on a table
-- a unique index exists on the table. The index must be based on single-key column, that does not allow NULL.
-- full-text catalog exists. You have to specify full-text catalog name explicitly if there is no default full-text catalog.


DECLARE @SearchWord NVARCHAR(30)  
SET @SearchWord = N'' 
SELECT [IlKod]
      ,[Il]
      ,[IlceKod]
      ,[Ilce]
      ,[SbbKod]
      ,[SemtBucakBelde]
      ,[MahalleKod]
      ,[Mahalle]
      ,[PostaKod]
  FROM [App101].[dbo].[IL_ILCE_SBB_MAHALLE] 
WHERE CONTAINS([Mahalle], @SearchWord);  


``` 


``` csharp

    public class Emre
    {
        internal class IL
        {
            public string IlKod { get; set; }
            public string Il { get; set; }
        }

        internal class ILCE
        {
            public string IlKod { get; set; }
            public string Il { get; set; }
            public string IlceKod { get; set; }
            public string Ilce { get; set; }
        }

        internal class SBB
        {
            public string IlKod { get; set; }
            public string Il { get; set; }
            public string IlceKod { get; set; }
            public string Ilce { get; set; }
            public string SbbKod { get; set; }
            public string SemtBucakBelde { get; set; }
        }

        internal class MAHALLE
        {
            public string IlKod { get; set; }
            public string Il { get; set; }
            public string IlceKod { get; set; }
            public string Ilce { get; set; }
            public string SbbKod { get; set; }
            public string SemtBucakBelde { get; set; }
            public string MahalleKod { get; set; }
            public string Mahalle { get; set; }
            public string PostaKod { get; set; }
        }

        public static void ExportJson()
        {
            ExportJsonIl();
            ExportJsonIlce();
            ExportJsonSBB();
            ExportJsonMahalle();
        }

        public static void ExportJsonIl()
        {
            List<IL> iller = new List<IL>();            
            
            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter("SELECT [IlKod] ,[Il] FROM[App101].[dbo].[ILLER]", "Server=.;Database=App101;User Id=sa;Password=aA123456;");

            da.Fill(dt);

            foreach(DataRow dr in dt.Rows)
            {
                iller.Add(new IL() { IlKod = dr["IlKod"].ToString().Trim(), Il = dr["Il"].ToString().Trim() });
            }

            List<IL> SortedList = iller.OrderBy(o => o.IlKod).ToList();

            string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(SortedList, Newtonsoft.Json.Formatting.Indented);

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "IL.json");

            File.WriteAllText(path, jsonText);

            //Product deserializedProduct = JsonConvert.DeserializeObject<Product>(json);
        }

        public static void ExportJsonIlce()
        {
            List<ILCE> iller = new List<ILCE>();

            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter("SELECT [IlKod] ,[Il], [IlceKod], [Ilce] FROM[App101].[dbo].[ILCELER]", "Server=.;Database=App101;User Id=sa;Password=aA123456;");

            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                iller.Add(new ILCE() { 
                    IlKod = dr["IlKod"].ToString().Trim(), 
                    Il = dr["Il"].ToString().Trim(),
                    IlceKod = dr["IlceKod"].ToString().Trim(),
                    Ilce = dr["Ilce"].ToString().Trim()

                });
            }

            List<ILCE> SortedList = iller.OrderBy(o => o.IlKod).ThenBy(o=>o.Ilce) .ToList();

            string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(SortedList, Newtonsoft.Json.Formatting.Indented);

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ILCE.json");

            File.WriteAllText(path, jsonText);

            //Product deserializedProduct = JsonConvert.DeserializeObject<Product>(json);
        }

        public static void ExportJsonSBB()
        {
            List<SBB> iller = new List<SBB>();

            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter("SELECT [IlKod] ,[Il], [IlceKod], [Ilce],SbbKod,SemtBucakBelde FROM[App101].[dbo].[SBB]", "Server=.;Database=App101;User Id=sa;Password=aA123456;");

            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                iller.Add(new SBB()
                {
                    IlKod = dr["IlKod"].ToString().Trim(),
                    Il = dr["Il"].ToString().Trim(),
                    IlceKod = dr["IlceKod"].ToString().Trim(),
                    Ilce = dr["Ilce"].ToString().Trim(),
                    SbbKod = dr["SbbKod"].ToString().Trim(),
                    SemtBucakBelde = dr["SemtBucakBelde"].ToString().Trim()

                });
            }

            List<SBB> SortedList = iller.OrderBy(o => o.IlKod).ThenBy(o => o.Ilce).ThenBy(o=>o.SemtBucakBelde).ToList();

            string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(SortedList, Newtonsoft.Json.Formatting.Indented);

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SBB.json");

            File.WriteAllText(path, jsonText);

            //Product deserializedProduct = JsonConvert.DeserializeObject<Product>(json);
        }

        public static void ExportJsonMahalle()
        {
            List<MAHALLE> iller = new List<MAHALLE>();

            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter("SELECT [IlKod] ,[Il], [IlceKod], [Ilce],SbbKod,SemtBucakBelde,MahalleKod, Mahalle, PostaKod FROM[App101].[dbo].[MAHALLE]", "Server=.;Database=App101;User Id=sa;Password=aA123456;");

            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                iller.Add(new MAHALLE()
                {
                    IlKod = dr["IlKod"].ToString().Trim(),
                    Il = dr["Il"].ToString().Trim(),
                    IlceKod = dr["IlceKod"].ToString().Trim(),
                    Ilce = dr["Ilce"].ToString().Trim(),
                    SbbKod = dr["SbbKod"].ToString().Trim(),
                    SemtBucakBelde = dr["SemtBucakBelde"].ToString().Trim(),
                    MahalleKod = dr["MahalleKod"].ToString().Trim(),
                    Mahalle = dr["Mahalle"].ToString().Trim(),
                    PostaKod = dr["PostaKod"].ToString().Trim()

                });
            }

            List<MAHALLE> SortedList = iller.OrderBy(o => o.IlKod).ThenBy(o => o.Ilce).ThenBy(o=>o.SemtBucakBelde).ThenBy(o=>o.Mahalle).ToList();

            string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(SortedList, Newtonsoft.Json.Formatting.Indented);

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "MAHALLE.json");

            File.WriteAllText(path, jsonText);

            //Product deserializedProduct = JsonConvert.DeserializeObject<Product>(json);
        }
    }




            // Options are defined in OnConfiguringmethod of AppDataContext
            services.AddDbContext<AppDataContext>();
            // services.AddDbContext<AppDataContext>(options => options.UseSqlite("Data Source=AppDB.db;"));

```


Try this:

DbContext.Configuration.ProxyCreationEnabled = true;    
DbContext.Configuration.LazyLoadingEnabled = true;  
If DbContext.Configuration.ProxyCreationEnabled is set to false, DbContext will not load child objects for some parent object unless Include method is called on parent object. Setting DbContext.Configuration.LazyLoadingEnabled to true or false will have no impact on its behaviours.

If DbContext.Configuration.ProxyCreationEnabled is set to true, child objects will be loaded automatically, and DbContext.Configuration.LazyLoadingEnabled value will control when child objects are loaded.

Steps To Configure Lazy Loading with Proxies in Asp.net Core 2.1

Install Microsoft.EntityFrameworkCore.Proxies package
Enable LazyLoadingProxies You can enable it with a call to UseLazyLoadingProxies:
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        .UseLazyLoadingProxies()
        .UseSqlServer(myConnectionString);
Or when using AddDbContext:

`.AddDbContext<BloggingContext>`(
b => b.UseLazyLoadingProxies()
      .UseSqlServer(myConnectionString));
EF Core will then enable lazy loading for any navigation property that can be overridden--that is, it must be virtual.

public void ConfigureServices(IServiceCollection services)
{
    #region Database configuration

    // Database configuration
    services.AddDbContext<DbContext>(options =>
        options.UseLazyLoadingProxies()
            .UseSqlServer(Configuration.GetConnectionString("MyConnectionString")));

    #endregion Database configuration
}

# Note

Eager loading means that the related data is loaded from the database as part of the initial query. You can use the Include method to specify related data to be included in query results.
    var blogs = context.Blogs.Include(blog => blog.Posts).ToList();
Explicit loading means that the related data is explicitly loaded from the database at a later time. You can explicitly load a navigation property via the DbContext.Entry(...) API.
    var blog = context.Blogs.Single(b => b.BlogId == 1);
    context.Entry(blog).Collection(b => b.Posts).Load();
    context.Entry(blog).Reference(b => b.Owner).Load();

Lazy loading means that the related data is transparently loaded from the database when the navigation property is accessed. The simplest way to use lazy-loading is by installing the Microsoft.EntityFrameworkCore.Proxies package and enabling it with a call to UseLazyLoadingProxies.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlServer(myConnectionString);
    Or when using AddDbContext:

    .AddDbContext<BloggingContext>(
        b => b.UseLazyLoadingProxies()
              .UseSqlServer(myConnectionString));

    Lazy-loading proxies work by injecting the ILazyLoader service into an entity, as described in Entity Type Constructors. 

Try this:

DbContext.Configuration.ProxyCreationEnabled = true;    
DbContext.Configuration.LazyLoadingEnabled = true;  
If DbContext.Configuration.ProxyCreationEnabled is set to false, DbContext will not load child objects for some parent object unless Include method is called on parent object. Setting DbContext.Configuration.LazyLoadingEnabled to true or false will have no impact on its behaviours.

If DbContext.Configuration.ProxyCreationEnabled is set to true, child objects will be loaded automatically, and DbContext.Configuration.LazyLoadingEnabled value will control when child objects are loaded.

Steps To Configure Lazy Loading with Proxies in Asp.net Core 2.1

Install Microsoft.EntityFrameworkCore.Proxies package
Enable LazyLoadingProxies You can enable it with a call to UseLazyLoadingProxies:
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        .UseLazyLoadingProxies()
        .UseSqlServer(myConnectionString);
Or when using AddDbContext:

`.AddDbContext<BloggingContext>`(
b => b.UseLazyLoadingProxies()
      .UseSqlServer(myConnectionString));
EF Core will then enable lazy loading for any navigation property that can be overridden--that is, it must be virtual.

public void ConfigureServices(IServiceCollection services)
{
    #region Database configuration

    // Database configuration
    services.AddDbContext<DbContext>(options =>
        options.UseLazyLoadingProxies()
            .UseSqlServer(Configuration.GetConnectionString("MyConnectionString")));

    #endregion Database configuration
}

# References

* https://www.entityframeworktutorial.net/
* https://www.learnentityframeworkcore.com/
* https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-5.0
* https://docs.microsoft.com/en-us/ef/core/querying/related-data/


# OpenJSON

* https://docs.microsoft.com/en-us/sql/t-sql/functions/openjson-transact-sql?view=sql-server-ver15
* https://www.sqlshack.com/importexport-json-data-using-sql-server-2016/
* https://docs.microsoft.com/en-us/sql/relational-databases/json/format-query-results-as-json-with-for-json-sql-server?view=sql-server-ver15
* https://docs.microsoft.com/en-us/sql/relational-databases/json/json-data-sql-server?view=sql-server-ver15


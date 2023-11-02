using Microsoft.EntityFrameworkCore;
using RoutineApi.Entities;

namespace RoutineApi.Data
{
    public class RoutineDbContext : DbContext
    {
        public RoutineDbContext(DbContextOptions<RoutineDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public static readonly ILoggerFactory ConsoleloggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                .AddConsole();
        });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(ConsoleloggerFactory).EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Employee>()
            //    .HasOne(x => x.Company)
            //    .WithMany(x => x.Employees)
            //    .HasForeignKey(x => x.CompanyId)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = Guid.Parse("49C1A7E1-0C79-4A89-A3D6-A37998FB8000"),
                    Name = "Macrohard",
                    Introduction = "Bigggggggggg"
                },
                new Company
                {
                    Id = Guid.Parse("49C1A7E1-0C79-4A89-A3D6-A37998FB8001"),
                    Name = "Apple",
                    Introduction = "Bad Dragon"
                },
                new Company
                {
                    Id = Guid.Parse("49C1A7E1-0C79-4A89-A3D6-A37998FB8002"),
                    Name = "Google",
                    Introduction = "Goodly Goods"
                },
                new Company
                {
                    Id = Guid.Parse("49C1A7E1-0C79-4A89-A3D6-A37998FB8003"),
                    Name = "Alipapa",
                    Introduction = "996 Company"
                },
                new Company
                {
                    Id = Guid.Parse("49C1A7E1-0C79-4A89-A3D6-A37998FB8004"),
                    Name = "PDD",
                    Introduction = "Fubao Company"
                },
                new Company
                {
                    Id = Guid.Parse("49C1A7E1-0C79-4A89-A3D6-A37998FB8E86"),
                    Name = "HuaWei",
                    Introduction = "Patriotic Company"
                }
                );

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = Guid.Parse("BBCC8B7A-CFB2-4D63-BB13-1A669129B81C"),
                    CompanyId = Guid.Parse("49C1A7E1-0C79-4A89-A3D6-A37998FB8000"),
                    DateOfBirth = DateTime.Now,
                    EmployeeNo = "MST100",
                    FirestName = "lee",
                    LastName = "huaaa",
                    Gender = Gender.male
                },
                new Employee
                {
                    Id = Guid.Parse("2E533A50-A67E-4933-B592-0B6E175F2C68"),
                    CompanyId = Guid.Parse("49C1A7E1-0C79-4A89-A3D6-A37998FB8001"),
                    DateOfBirth = DateTime.Now,
                    EmployeeNo = "ApNo100",
                    FirestName = "Shell",
                    LastName = "huaaa",
                    Gender = Gender.male
                },
                new Employee
                {
                    Id = Guid.Parse("DA81469F-D68B-48E8-B3CB-774FA1EFE0F0"),
                    CompanyId = Guid.Parse("49C1A7E1-0C79-4A89-A3D6-A37998FB8002"),
                    DateOfBirth = DateTime.Now,
                    EmployeeNo = "GoNo100",
                    FirestName = "lee",
                    LastName = "huaaa",
                    Gender = Gender.female
                },
                new Employee
                {
                    Id = Guid.Parse("5CC28956-45B0-4F90-8316-C8567D0A4D79"),
                    CompanyId = Guid.Parse("49C1A7E1-0C79-4A89-A3D6-A37998FB8E86"),
                    DateOfBirth = DateTime.Now,
                    EmployeeNo = "HW251",
                    FirestName = "hong",
                    LastName = "Tianyee",
                    Gender = Gender.male
                }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}

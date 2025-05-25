using CarService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<CarCustomer> CarCustomers { get; set; }
        public DbSet<RepairEmployee> RepairEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Brand)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.Model)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.RegistrationNumber)
                      .HasMaxLength(50);
                entity.Property(e => e.Vin)
                      .HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.SurName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.PhoneNumber)
                      .HasMaxLength(50);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.SurName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.PhoneNumber)
                      .HasMaxLength(50);
                entity.Property(e => e.Position)
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Repair>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description)
                      .HasMaxLength(1000);
                entity.Property(e => e.Price)
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(r => r.Car)
                      .WithMany()
                      .HasForeignKey(r => r.CarId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CarCustomer>()
                .HasKey(cc => new { cc.CarId, cc.CustomerId });

            modelBuilder.Entity<CarCustomer>()
                .HasOne(cc => cc.Car)
                .WithMany(c => c.CarCustomers)
                .HasForeignKey(cc => cc.CarId);

            modelBuilder.Entity<CarCustomer>()
                .HasOne(cc => cc.Customer)
                .WithMany(c => c.CarCustomers)
                .HasForeignKey(cc => cc.CustomerId);

            modelBuilder.Entity<RepairEmployee>()
                .HasKey(re => new { re.RepairId, re.EmployeeId });

            modelBuilder.Entity<RepairEmployee>()
                .HasOne(re => re.Repair)
                .WithMany(r => r.RepairEmployees)
                .HasForeignKey(re => re.RepairId);

            modelBuilder.Entity<RepairEmployee>()
                .HasOne(re => re.Employee)
                .WithMany(e => e.RepairEmployees)
                .HasForeignKey(re => re.EmployeeId);
        }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
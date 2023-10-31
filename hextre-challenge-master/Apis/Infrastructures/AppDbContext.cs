using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructures
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext()
        {
            
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Chemical> Chemicals { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CancelReason> CancelReason { get; set; }
        public DbSet<DepositPayment> DepositPayments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Provider> Provider { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<ImageWarehouse> ImageWarehouse { get; set; }
        public DbSet<WarehouseDetail> WarehouseDetail { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PostHashtag> PostHashtag { get; set; }
        public DbSet<RentWarehouse> RentWarehouse { get; set; }

        //public DbSet<DepositPayment> DepositPayment { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json", true, true).Build();
            var strConn = config["ConnectionStrings:Development"];
            return strConn;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }
    }
}

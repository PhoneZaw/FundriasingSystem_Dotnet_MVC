using FundraisingApp.Entities;
using FundriasingSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace FundriasingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Staff> Staff { get; set; }
        public DbSet<StaffRole> StaffRoles { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<CampaignType> CampaignTypes { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
    }
}

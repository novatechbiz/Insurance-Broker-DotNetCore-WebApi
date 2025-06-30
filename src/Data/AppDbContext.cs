using Microsoft.EntityFrameworkCore;

namespace InsuraNova.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<CustomerIdentificationType> CustomerIdentificationTypes { get; set; }
        public DbSet<GenderType> GenderTypes { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<EntryType> EntryTypes{ get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<InsuranceType> InsuranceTypes { get; set; }
        public DbSet<RecordStatus> RecordStatuses { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<SystemFunction> SystemFunctions { get; set; }
        public DbSet<PremiumLine> PremiumLines { get; set; }
        public DbSet<InsuranceCompany> InsuranceCompanies { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerHistoryLog> CustomerHisrotyLogs { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

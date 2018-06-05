using Microsoft.EntityFrameworkCore;
 
namespace Bank_Accounts.Models
{
    public class BankAccountContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public BankAccountContext(DbContextOptions<BankAccountContext> options) : base(options) { }
        public DbSet<User> users { get; set; }
        public DbSet<Transaction> transactions { get; set; }
    }
}
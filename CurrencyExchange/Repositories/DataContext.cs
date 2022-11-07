using CurrencyExchange.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Repositories;

public class DataContext : DbContext
{
        public DataContext(DbContextOptions<DataContext> options):base(options) {  }
        public DbSet<Trade> Trades { get; set; }
}
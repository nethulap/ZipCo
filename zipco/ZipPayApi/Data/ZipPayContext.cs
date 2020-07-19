using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ZipPayApi.Data
{
    public class ZipPayContext : DbContext
    {
        private static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        private readonly string ConnectionString;
        public ZipPayContext(DbContextOptions<ZipPayContext> options, IConfiguration configuration) : base(options)
        {
            this.ConnectionString = configuration.GetConnectionString("ZipPayDBConnection");
        }

        public DbSet<User> User { get; set; }
        public DbSet<Account> Account { get; set; }
    }
}

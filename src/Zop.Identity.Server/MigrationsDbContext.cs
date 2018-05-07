using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Reflection;
using Zop.Repositories.Configuration;

namespace Zop.Identity.Server
{
    public class MigrationsDbContext : RepositoryDbContext
    {
        public static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });
        public MigrationsDbContext():base(null, MyLoggerFactory)
        {
           
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            string connectionString = @"Database=zop_ids;Data Source=120.78.175.212;User Id=root;Password=zwcsroot;pooling=false";
            optionsBuilder.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
        }

    }
}

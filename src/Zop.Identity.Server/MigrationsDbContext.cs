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
            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
            string connectionString = @"Database=Test;Data Source=127.0.0.1;User Id=root;Password=sapass;pooling=false";
            optionsBuilder.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
        }

    }
}

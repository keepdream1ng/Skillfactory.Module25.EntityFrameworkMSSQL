using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillfactory.Module25.EntityFrameworkMSSQL
{
    public class AppContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // I'm running a docker linux container instead of server on windows for study purposes.
            optionsBuilder.UseSqlServer(@"Server=localhost,1433;Database=EF;User Id=SA;Password=Password1sSafe!;TrustServerCertificate=true;");
        }
    }
}

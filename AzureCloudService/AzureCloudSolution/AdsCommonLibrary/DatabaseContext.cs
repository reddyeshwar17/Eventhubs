using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsCommonLibrary
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=DefaultConnection")
        {

        }
        public DatabaseContext(string connectionString):base(connectionString)
        {

        }

        public DbSet<Ad> Ads { get; set; }
    }
}

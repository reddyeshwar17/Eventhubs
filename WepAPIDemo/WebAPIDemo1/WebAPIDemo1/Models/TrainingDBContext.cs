using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebAPIDemo1.Models
{
    public class TrainingDBContext : DbContext
    {
        public TrainingDBContext() : base("name=TrainingDB")
        {
        }

        public DbSet<PersonalDetail> PersonalDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
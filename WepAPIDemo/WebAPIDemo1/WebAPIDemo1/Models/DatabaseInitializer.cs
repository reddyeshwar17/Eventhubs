using System.Data.Entity;

namespace WebAPIDemo1.Models
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<StudentContext>
    {
        protected override void Seed(StudentContext context)
        {
            base.Seed(context);
        }
    }
}
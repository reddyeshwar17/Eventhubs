using System.Data.Entity;

namespace WebAPIDemo1.Models
{
    public class DatabaseInitializer1 : DropCreateDatabaseIfModelChanges<TrainingDBContext>
    {
        protected override void Seed(TrainingDBContext context)
        {
            base.Seed(context);
        }
    }
}
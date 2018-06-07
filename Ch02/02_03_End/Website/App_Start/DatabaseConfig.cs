using HPlusSports.Models;
using System.Data.Entity;

namespace HPlusSports.App_Start
{
    public class DatabaseConfig
    {
        public static void Initialize()
        {
            Database.SetInitializer(new HPlusSportsDbContextInitializer());
            var context = new HPlusSportsDbContext();
            context.Database.Initialize(true);
        }
    }
}
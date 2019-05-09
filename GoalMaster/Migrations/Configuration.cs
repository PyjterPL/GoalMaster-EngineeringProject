namespace GoalMaster.Migrations
{
    using GoalMaster.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GoalMasterDatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GoalMaster.Model.GoalMasterDatabaseContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.GoalTypes.AddOrUpdate(x => x.ID,
                new GoalType() { ID = 1, Description = "Done or not done" },
                new GoalType() { ID = 2, Description = "Numeric values" }
            );
            context.RelationshipStatuses.AddOrUpdate(x => x.ID,
                new RelationshipStatus() { ID = 1, Description = "Pending" },
                new RelationshipStatus() { ID = 2, Description = "Accepted" },
                new RelationshipStatus() { ID = 3, Description = "Declined" },
                new RelationshipStatus() { ID = 4, Description = "Blocked" }
                );

        }
    }
}

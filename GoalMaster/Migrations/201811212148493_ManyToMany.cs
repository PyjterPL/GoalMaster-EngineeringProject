namespace GoalMaster.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToMany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GoalDefinitions", "OwnerUserID_ID", "dbo.Users");
            AddColumn("dbo.GoalDefinitions", "User_ID", c => c.Int());
            AddColumn("dbo.Users", "GoalDefinition_ID", c => c.Int());
            CreateIndex("dbo.GoalDefinitions", "User_ID");
            CreateIndex("dbo.Users", "GoalDefinition_ID");
            AddForeignKey("dbo.Users", "GoalDefinition_ID", "dbo.GoalDefinitions", "ID");
            AddForeignKey("dbo.GoalDefinitions", "User_ID", "dbo.Users", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoalDefinitions", "User_ID", "dbo.Users");
            DropForeignKey("dbo.Users", "GoalDefinition_ID", "dbo.GoalDefinitions");
            DropIndex("dbo.Users", new[] { "GoalDefinition_ID" });
            DropIndex("dbo.GoalDefinitions", new[] { "User_ID" });
            DropColumn("dbo.Users", "GoalDefinition_ID");
            DropColumn("dbo.GoalDefinitions", "User_ID");
            AddForeignKey("dbo.GoalDefinitions", "OwnerUserID_ID", "dbo.Users", "ID");
        }
    }
}

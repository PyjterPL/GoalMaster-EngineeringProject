namespace GoalMaster.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectionTablesUserGoalDefinition : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GoalDefinitions", "User_ID", "dbo.Users");
            DropForeignKey("dbo.Users", "GoalDefinition_ID", "dbo.GoalDefinitions");
            DropIndex("dbo.GoalDefinitions", new[] { "User_ID" });
            DropIndex("dbo.Users", new[] { "GoalDefinition_ID" });
            CreateTable(
                "dbo.GoalDefinitionsUsers",
                c => new
                    {
                        UserRefId = c.Int(nullable: false),
                        GoalDefinitionRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserRefId, t.GoalDefinitionRefId })
                .ForeignKey("dbo.Users", t => t.UserRefId, cascadeDelete: true)
                .ForeignKey("dbo.GoalDefinitions", t => t.GoalDefinitionRefId, cascadeDelete: true)
                .Index(t => t.UserRefId)
                .Index(t => t.GoalDefinitionRefId);
            
            DropColumn("dbo.GoalDefinitions", "User_ID");
            DropColumn("dbo.Users", "GoalDefinition_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "GoalDefinition_ID", c => c.Int());
            AddColumn("dbo.GoalDefinitions", "User_ID", c => c.Int());
            DropForeignKey("dbo.GoalDefinitionsUsers", "GoalDefinitionRefId", "dbo.GoalDefinitions");
            DropForeignKey("dbo.GoalDefinitionsUsers", "UserRefId", "dbo.Users");
            DropIndex("dbo.GoalDefinitionsUsers", new[] { "GoalDefinitionRefId" });
            DropIndex("dbo.GoalDefinitionsUsers", new[] { "UserRefId" });
            DropTable("dbo.GoalDefinitionsUsers");
            CreateIndex("dbo.Users", "GoalDefinition_ID");
            CreateIndex("dbo.GoalDefinitions", "User_ID");
            AddForeignKey("dbo.Users", "GoalDefinition_ID", "dbo.GoalDefinitions", "ID");
            AddForeignKey("dbo.GoalDefinitions", "User_ID", "dbo.Users", "ID");
        }
    }
}

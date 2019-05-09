namespace GoalMaster.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GoalDefinitions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Shared = c.Boolean(nullable: false),
                        GoalType_ID = c.Int(),
                        OwnerUserID_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GoalTypes", t => t.GoalType_ID)
                .ForeignKey("dbo.Users", t => t.OwnerUserID_ID)
                .Index(t => t.GoalType_ID)
                .Index(t => t.OwnerUserID_ID);
            
            CreateTable(
                "dbo.GoalTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                        Mail = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.GoalRecords",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Value = c.Int(nullable: false),
                        Note = c.String(),
                        GoalDefinition_ID = c.Int(),
                        User_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GoalDefinitions", t => t.GoalDefinition_ID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.GoalDefinition_ID)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.Relationships",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ActionUser_ID = c.Int(),
                        Status_ID = c.Int(),
                        UserOne_ID = c.Int(),
                        UserTwo_ID = c.Int(),
                        User_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.ActionUser_ID)
                .ForeignKey("dbo.Statuses", t => t.Status_ID)
                .ForeignKey("dbo.Users", t => t.UserOne_ID)
                .ForeignKey("dbo.Users", t => t.UserTwo_ID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.ActionUser_ID)
                .Index(t => t.Status_ID)
                .Index(t => t.UserOne_ID)
                .Index(t => t.UserTwo_ID)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.Statuses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.GoalMembers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GoalDefinition_ID = c.Int(),
                        User_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GoalDefinitions", t => t.GoalDefinition_ID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.GoalDefinition_ID)
                .Index(t => t.User_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoalMembers", "User_ID", "dbo.Users");
            DropForeignKey("dbo.GoalMembers", "GoalDefinition_ID", "dbo.GoalDefinitions");
            DropForeignKey("dbo.Relationships", "User_ID", "dbo.Users");
            DropForeignKey("dbo.Relationships", "UserTwo_ID", "dbo.Users");
            DropForeignKey("dbo.Relationships", "UserOne_ID", "dbo.Users");
            DropForeignKey("dbo.Relationships", "Status_ID", "dbo.Statuses");
            DropForeignKey("dbo.Relationships", "ActionUser_ID", "dbo.Users");
            DropForeignKey("dbo.GoalRecords", "User_ID", "dbo.Users");
            DropForeignKey("dbo.GoalRecords", "GoalDefinition_ID", "dbo.GoalDefinitions");
            DropForeignKey("dbo.GoalDefinitions", "OwnerUserID_ID", "dbo.Users");
            DropForeignKey("dbo.GoalDefinitions", "GoalType_ID", "dbo.GoalTypes");
            DropIndex("dbo.GoalMembers", new[] { "User_ID" });
            DropIndex("dbo.GoalMembers", new[] { "GoalDefinition_ID" });
            DropIndex("dbo.Relationships", new[] { "User_ID" });
            DropIndex("dbo.Relationships", new[] { "UserTwo_ID" });
            DropIndex("dbo.Relationships", new[] { "UserOne_ID" });
            DropIndex("dbo.Relationships", new[] { "Status_ID" });
            DropIndex("dbo.Relationships", new[] { "ActionUser_ID" });
            DropIndex("dbo.GoalRecords", new[] { "User_ID" });
            DropIndex("dbo.GoalRecords", new[] { "GoalDefinition_ID" });
            DropIndex("dbo.GoalDefinitions", new[] { "OwnerUserID_ID" });
            DropIndex("dbo.GoalDefinitions", new[] { "GoalType_ID" });
            DropTable("dbo.GoalMembers");
            DropTable("dbo.Statuses");
            DropTable("dbo.Relationships");
            DropTable("dbo.GoalRecords");
            DropTable("dbo.Users");
            DropTable("dbo.GoalTypes");
            DropTable("dbo.GoalDefinitions");
        }
    }
}

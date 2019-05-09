namespace GoalMaster.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsersInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProfileImage = c.Binary(),
                        UserDescription = c.String(),
                        Address = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        User_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.User_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersInfo", "User_ID", "dbo.Users");
            DropIndex("dbo.UsersInfo", new[] { "User_ID" });
            DropTable("dbo.UsersInfo");
        }
    }
}

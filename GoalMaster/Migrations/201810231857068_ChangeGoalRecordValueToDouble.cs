namespace GoalMaster.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeGoalRecordValueToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GoalRecords", "Value", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GoalRecords", "Value", c => c.Int(nullable: false));
        }
    }
}

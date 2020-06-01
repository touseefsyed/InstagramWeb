namespace InstagramWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSchedule : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Schedules.Schedule", "PostedStatus", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("Schedules.Schedule", "PostedStatus", c => c.Boolean(nullable: false));
        }
    }
}

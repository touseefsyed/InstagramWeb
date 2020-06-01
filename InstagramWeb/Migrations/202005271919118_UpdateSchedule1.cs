namespace InstagramWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSchedule1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Schedules.Schedule", "LastTryDate", c => c.DateTime());
            AddColumn("Schedules.Schedule", "Exception", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Schedules.Schedule", "Exception");
            DropColumn("Schedules.Schedule", "LastTryDate");
        }
    }
}

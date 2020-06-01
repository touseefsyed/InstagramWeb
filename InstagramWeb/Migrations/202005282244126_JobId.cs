namespace InstagramWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobId : DbMigration
    {
        public override void Up()
        {
            AddColumn("Schedules.Schedule", "JobId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Schedules.Schedule", "JobId");
        }
    }
}

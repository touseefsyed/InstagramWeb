namespace InstagramWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Instapost1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Schedules.Schedule", "TimeStamp", c => c.DateTime(nullable: false));
            AddColumn("Schedules.Schedule", "PostedStatus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Schedules.Schedule", "PostedStatus");
            DropColumn("Schedules.Schedule", "TimeStamp");
        }
    }
}

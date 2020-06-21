namespace InstagramWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Story : DbMigration
    {
        public override void Up()
        {
            AddColumn("Schedules.Schedule", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Schedules.Schedule", "Type");
        }
    }
}

namespace InstagramWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScheduleUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("Schedules.Schedule", "UserId", c => c.Int(nullable: false));
            CreateIndex("Schedules.Schedule", "UserId");
            AddForeignKey("Schedules.Schedule", "UserId", "Accounts.User", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Schedules.Schedule", "UserId", "Accounts.User");
            DropIndex("Schedules.Schedule", new[] { "UserId" });
            DropColumn("Schedules.Schedule", "UserId");
        }
    }
}

namespace InstagramWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Instapost : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Schedules.ScheduleImage", "ScheduleID", "Schedules.Schedule");
            DropForeignKey("Schedules.ScheduleVideo", "ScheduleID", "Schedules.Schedule");
            DropIndex("Schedules.ScheduleImage", new[] { "ScheduleID" });
            DropIndex("Schedules.ScheduleVideo", new[] { "ScheduleID" });
            AddColumn("Schedules.Schedule", "Caption", c => c.String());
            AddColumn("Schedules.Schedule", "ImagePath", c => c.String());
            DropColumn("Schedules.Schedule", "FileName");
            DropColumn("Schedules.Schedule", "Path");
            DropColumn("Schedules.Schedule", "Title");
            DropColumn("Schedules.Schedule", "Description");
            DropTable("Schedules.ScheduleImage");
            DropTable("Schedules.ScheduleVideo");
        }
        
        public override void Down()
        {
            CreateTable(
                "Schedules.ScheduleVideo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        videoPath = c.String(),
                        ScheduleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "Schedules.ScheduleImage",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        imagePath = c.String(),
                        ScheduleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("Schedules.Schedule", "Description", c => c.String());
            AddColumn("Schedules.Schedule", "Title", c => c.String());
            AddColumn("Schedules.Schedule", "Path", c => c.String());
            AddColumn("Schedules.Schedule", "FileName", c => c.String());
            DropColumn("Schedules.Schedule", "ImagePath");
            DropColumn("Schedules.Schedule", "Caption");
            CreateIndex("Schedules.ScheduleVideo", "ScheduleID");
            CreateIndex("Schedules.ScheduleImage", "ScheduleID");
            AddForeignKey("Schedules.ScheduleVideo", "ScheduleID", "Schedules.Schedule", "ID", cascadeDelete: true);
            AddForeignKey("Schedules.ScheduleImage", "ScheduleID", "Schedules.Schedule", "ID", cascadeDelete: true);
        }
    }
}

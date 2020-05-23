namespace InstagramWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Schedules.Schedule",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Path = c.String(),
                        Title = c.String(),
                        Description = c.String(),
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("Schedules.Schedule", t => t.ScheduleID, cascadeDelete: true)
                .Index(t => t.ScheduleID);
            
            CreateTable(
                "Schedules.ScheduleVideo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        videoPath = c.String(),
                        ScheduleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Schedules.Schedule", t => t.ScheduleID, cascadeDelete: true)
                .Index(t => t.ScheduleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Schedules.ScheduleVideo", "ScheduleID", "Schedules.Schedule");
            DropForeignKey("Schedules.ScheduleImage", "ScheduleID", "Schedules.Schedule");
            DropIndex("Schedules.ScheduleVideo", new[] { "ScheduleID" });
            DropIndex("Schedules.ScheduleImage", new[] { "ScheduleID" });
            DropTable("Schedules.ScheduleVideo");
            DropTable("Schedules.ScheduleImage");
            DropTable("Schedules.Schedule");
        }
    }
}

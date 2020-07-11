namespace InstagramWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GetFollowersCountRecorder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Accounts.DailyFollowerCount",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RecordedDate = c.DateTime(nullable: false),
                        Followers = c.Int(nullable: false),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Accounts.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Accounts.DailyFollowerCount", "UserId", "Accounts.User");
            DropIndex("Accounts.DailyFollowerCount", new[] { "UserId" });
            DropTable("Accounts.DailyFollowerCount");
        }
    }
}

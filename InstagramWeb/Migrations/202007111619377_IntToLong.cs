namespace InstagramWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntToLong : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Accounts.DailyFollowerCount", "Followers", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Accounts.DailyFollowerCount", "Followers", c => c.Int(nullable: false));
        }
    }
}

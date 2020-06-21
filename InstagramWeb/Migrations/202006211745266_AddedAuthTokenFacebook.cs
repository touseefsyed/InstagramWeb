namespace InstagramWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAuthTokenFacebook : DbMigration
    {
        public override void Up()
        {
            AddColumn("Accounts.User", "InstagramUserId", c => c.String());
            AddColumn("Accounts.User", "InstagramAuthToken", c => c.String());
            AddColumn("Accounts.User", "FacebookAuthToken", c => c.String());
            AddColumn("Accounts.User", "FacebookPageId", c => c.String());
            DropColumn("Accounts.User", "AuthToken");
        }
        
        public override void Down()
        {
            AddColumn("Accounts.User", "AuthToken", c => c.String());
            DropColumn("Accounts.User", "FacebookPageId");
            DropColumn("Accounts.User", "FacebookAuthToken");
            DropColumn("Accounts.User", "InstagramAuthToken");
            DropColumn("Accounts.User", "InstagramUserId");
        }
    }
}

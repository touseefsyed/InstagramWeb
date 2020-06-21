namespace InstagramWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAuthToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("Accounts.User", "AuthToken", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Accounts.User", "AuthToken");
        }
    }
}

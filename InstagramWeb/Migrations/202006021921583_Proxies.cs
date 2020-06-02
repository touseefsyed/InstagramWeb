namespace InstagramWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Proxies : DbMigration
    {
        public override void Up()
        {
            AddColumn("Accounts.Proxy", "Username", c => c.String());
            AddColumn("Accounts.Proxy", "Password", c => c.String());
            AlterColumn("Accounts.Proxy", "IpAddress", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Accounts.Proxy", "IpAddress", c => c.String());
            DropColumn("Accounts.Proxy", "Password");
            DropColumn("Accounts.Proxy", "Username");
        }
    }
}

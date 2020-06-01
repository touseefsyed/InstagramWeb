namespace InstagramWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Users1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Users", newName: "User");
            MoveTable(name: "dbo.User", newSchema: "Accounts");
            CreateTable(
                "Accounts.Proxy",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Country = c.String(),
                        IpAddress = c.String(),
                        Timeouts = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("Accounts.User", "InstagramUsername", c => c.String());
            AddColumn("Accounts.User", "InstagramPassword", c => c.String());
            AddColumn("Accounts.User", "ProxyId", c => c.Int());
            CreateIndex("Accounts.User", "ProxyId");
            AddForeignKey("Accounts.User", "ProxyId", "Accounts.Proxy", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Accounts.User", "ProxyId", "Accounts.Proxy");
            DropIndex("Accounts.User", new[] { "ProxyId" });
            DropColumn("Accounts.User", "ProxyId");
            DropColumn("Accounts.User", "InstagramPassword");
            DropColumn("Accounts.User", "InstagramUsername");
            DropTable("Accounts.Proxy");
            MoveTable(name: "Accounts.User", newSchema: "dbo");
            RenameTable(name: "dbo.User", newName: "Users");
        }
    }
}

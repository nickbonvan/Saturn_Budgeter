namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resettables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Created = c.DateTimeOffset(nullable: false, precision: 7),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AccountTypeId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccountTypes", t => t.AccountTypeId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.AccountTypeId)
                .Index(t => t.User_Id);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BankAccounts", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.BankAccounts", "AccountTypeId", "dbo.AccountTypes");
            DropIndex("dbo.BankAccounts", new[] { "User_Id" });
            DropIndex("dbo.BankAccounts", new[] { "AccountTypeId" });
            DropTable("dbo.BankAccounts");
        }
    }
}

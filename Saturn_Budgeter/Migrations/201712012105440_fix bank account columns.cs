namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixbankaccountcolumns : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BankAccounts", new[] { "User_Id" });
            DropColumn("dbo.BankAccounts", "UserId");
            RenameColumn(table: "dbo.BankAccounts", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.BankAccounts", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.BankAccounts", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BankAccounts", new[] { "UserId" });
            AlterColumn("dbo.BankAccounts", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.BankAccounts", name: "UserId", newName: "User_Id");
            AddColumn("dbo.BankAccounts", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.BankAccounts", "User_Id");
        }
    }
}

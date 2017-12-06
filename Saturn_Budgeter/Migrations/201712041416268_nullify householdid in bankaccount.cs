namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullifyhouseholdidinbankaccount : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BankAccounts", "HouseholdId", "dbo.Households");
            DropIndex("dbo.BankAccounts", new[] { "HouseholdId" });
            AlterColumn("dbo.BankAccounts", "HouseholdId", c => c.Int());
            CreateIndex("dbo.BankAccounts", "HouseholdId");
            AddForeignKey("dbo.BankAccounts", "HouseholdId", "dbo.Households", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BankAccounts", "HouseholdId", "dbo.Households");
            DropIndex("dbo.BankAccounts", new[] { "HouseholdId" });
            AlterColumn("dbo.BankAccounts", "HouseholdId", c => c.Int(nullable: false));
            CreateIndex("dbo.BankAccounts", "HouseholdId");
            AddForeignKey("dbo.BankAccounts", "HouseholdId", "dbo.Households", "Id", cascadeDelete: true);
        }
    }
}

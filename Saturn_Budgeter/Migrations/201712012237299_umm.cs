namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class umm : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Invitations", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Invitations", new[] { "ApplicationUser_Id" });
            AddColumn("dbo.BankAccounts", "HouseholdId", c => c.Int(nullable: false));
            AlterColumn("dbo.Budgets", "BudgetBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.BankAccounts", "HouseholdId");
            AddForeignKey("dbo.BankAccounts", "HouseholdId", "dbo.Households", "Id", cascadeDelete: true);
            DropColumn("dbo.Invitations", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Invitations", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.BankAccounts", "HouseholdId", "dbo.Households");
            DropIndex("dbo.BankAccounts", new[] { "HouseholdId" });
            AlterColumn("dbo.Budgets", "BudgetBalance", c => c.Int(nullable: false));
            DropColumn("dbo.BankAccounts", "HouseholdId");
            CreateIndex("dbo.Invitations", "ApplicationUser_Id");
            AddForeignKey("dbo.Invitations", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}

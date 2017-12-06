namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamebudgetfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Budgets", "Name", c => c.String());
            AddColumn("dbo.Budgets", "Description", c => c.String());
            AddColumn("dbo.Budgets", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Budgets", "Created", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("dbo.Budgets", "BudgetName");
            DropColumn("dbo.Budgets", "BudgetDescription");
            DropColumn("dbo.Budgets", "BudgetBalance");
            DropColumn("dbo.Budgets", "BudgetCreated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Budgets", "BudgetCreated", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Budgets", "BudgetBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Budgets", "BudgetDescription", c => c.String());
            AddColumn("dbo.Budgets", "BudgetName", c => c.String());
            DropColumn("dbo.Budgets", "Created");
            DropColumn("dbo.Budgets", "Balance");
            DropColumn("dbo.Budgets", "Description");
            DropColumn("dbo.Budgets", "Name");
        }
    }
}

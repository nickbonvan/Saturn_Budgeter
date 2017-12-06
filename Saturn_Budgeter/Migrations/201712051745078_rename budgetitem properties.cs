namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamebudgetitemproperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BudgetItems", "Name", c => c.String());
            AddColumn("dbo.BudgetItems", "Description", c => c.String());
            AddColumn("dbo.BudgetItems", "Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BudgetItems", "Created", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.BudgetItems", "Updated", c => c.DateTimeOffset(precision: 7));
            DropColumn("dbo.BudgetItems", "ItemName");
            DropColumn("dbo.BudgetItems", "ItemDescription");
            DropColumn("dbo.BudgetItems", "ItemValue");
            DropColumn("dbo.BudgetItems", "ItemCreated");
            DropColumn("dbo.BudgetItems", "ItemUpdated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BudgetItems", "ItemUpdated", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.BudgetItems", "ItemCreated", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.BudgetItems", "ItemValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BudgetItems", "ItemDescription", c => c.String());
            AddColumn("dbo.BudgetItems", "ItemName", c => c.String());
            DropColumn("dbo.BudgetItems", "Updated");
            DropColumn("dbo.BudgetItems", "Created");
            DropColumn("dbo.BudgetItems", "Value");
            DropColumn("dbo.BudgetItems", "Description");
            DropColumn("dbo.BudgetItems", "Name");
        }
    }
}

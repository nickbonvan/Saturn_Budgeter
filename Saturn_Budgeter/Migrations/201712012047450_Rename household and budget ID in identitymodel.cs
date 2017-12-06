namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamehouseholdandbudgetIDinidentitymodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Budget_Id", "dbo.Budgets");
            DropForeignKey("dbo.AspNetUsers", "Household_Id", "dbo.Households");
            DropIndex("dbo.AspNetUsers", new[] { "Household_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Budget_Id" });
            RenameColumn(table: "dbo.AspNetUsers", name: "Budget_Id", newName: "BudgetId");
            RenameColumn(table: "dbo.AspNetUsers", name: "Household_Id", newName: "HouseholdId");
            AlterColumn("dbo.AspNetUsers", "HouseholdId", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "BudgetId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "HouseholdId");
            CreateIndex("dbo.AspNetUsers", "BudgetId");
            AddForeignKey("dbo.AspNetUsers", "BudgetId", "dbo.Budgets", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "HouseholdId", "dbo.Households", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "HouseholdId", "dbo.Households");
            DropForeignKey("dbo.AspNetUsers", "BudgetId", "dbo.Budgets");
            DropIndex("dbo.AspNetUsers", new[] { "BudgetId" });
            DropIndex("dbo.AspNetUsers", new[] { "HouseholdId" });
            AlterColumn("dbo.AspNetUsers", "BudgetId", c => c.Int());
            AlterColumn("dbo.AspNetUsers", "HouseholdId", c => c.Int());
            RenameColumn(table: "dbo.AspNetUsers", name: "HouseholdId", newName: "Household_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "BudgetId", newName: "Budget_Id");
            CreateIndex("dbo.AspNetUsers", "Budget_Id");
            CreateIndex("dbo.AspNetUsers", "Household_Id");
            AddForeignKey("dbo.AspNetUsers", "Household_Id", "dbo.Households", "Id");
            AddForeignKey("dbo.AspNetUsers", "Budget_Id", "dbo.Budgets", "Id");
        }
    }
}

namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Allowuserbudgetidtobenullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "BudgetId", "dbo.Budgets");
            DropIndex("dbo.AspNetUsers", new[] { "BudgetId" });
            AlterColumn("dbo.AspNetUsers", "BudgetId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "BudgetId");
            AddForeignKey("dbo.AspNetUsers", "BudgetId", "dbo.Budgets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "BudgetId", "dbo.Budgets");
            DropIndex("dbo.AspNetUsers", new[] { "BudgetId" });
            AlterColumn("dbo.AspNetUsers", "BudgetId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "BudgetId");
            AddForeignKey("dbo.AspNetUsers", "BudgetId", "dbo.Budgets", "Id", cascadeDelete: true);
        }
    }
}

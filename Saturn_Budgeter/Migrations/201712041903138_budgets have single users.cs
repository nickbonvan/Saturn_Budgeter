namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class budgetshavesingleusers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "BudgetId", "dbo.Budgets");
            DropIndex("dbo.AspNetUsers", new[] { "BudgetId" });
            AddColumn("dbo.Budgets", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Budgets", "UserId");
            AddForeignKey("dbo.Budgets", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Budgets", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Budgets", new[] { "UserId" });
            DropColumn("dbo.Budgets", "UserId");
            CreateIndex("dbo.AspNetUsers", "BudgetId");
            AddForeignKey("dbo.AspNetUsers", "BudgetId", "dbo.Budgets", "Id");
        }
    }
}

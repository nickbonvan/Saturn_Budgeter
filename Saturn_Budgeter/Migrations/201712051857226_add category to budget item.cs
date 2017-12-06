namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcategorytobudgetitem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BudgetItems", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.BudgetItems", "CategoryId");
            AddForeignKey("dbo.BudgetItems", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BudgetItems", "CategoryId", "dbo.Categories");
            DropIndex("dbo.BudgetItems", new[] { "CategoryId" });
            DropColumn("dbo.BudgetItems", "CategoryId");
        }
    }
}
namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Distinguishbudgetandbudgetitem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BudgetItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemName = c.String(),
                        ItemDescription = c.String(),
                        ItemValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemCreated = c.DateTimeOffset(nullable: false, precision: 7),
                        ItemUpdated = c.DateTimeOffset(precision: 7),
                        HouseholdId = c.Int(),
                        BudgetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Budgets", t => t.BudgetId, cascadeDelete: true)
                .ForeignKey("dbo.Households", t => t.HouseholdId)
                .Index(t => t.HouseholdId)
                .Index(t => t.BudgetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BudgetItems", "HouseholdId", "dbo.Households");
            DropForeignKey("dbo.BudgetItems", "BudgetId", "dbo.Budgets");
            DropIndex("dbo.BudgetItems", new[] { "BudgetId" });
            DropIndex("dbo.BudgetItems", new[] { "HouseholdId" });
            DropTable("dbo.BudgetItems");
        }
    }
}

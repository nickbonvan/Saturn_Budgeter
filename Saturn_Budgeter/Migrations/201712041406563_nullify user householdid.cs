namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullifyuserhouseholdid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "HouseholdId", "dbo.Households");
            DropIndex("dbo.AspNetUsers", new[] { "HouseholdId" });
            AlterColumn("dbo.AspNetUsers", "HouseholdId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "HouseholdId");
            AddForeignKey("dbo.AspNetUsers", "HouseholdId", "dbo.Households", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "HouseholdId", "dbo.Households");
            DropIndex("dbo.AspNetUsers", new[] { "HouseholdId" });
            AlterColumn("dbo.AspNetUsers", "HouseholdId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "HouseholdId");
            AddForeignKey("dbo.AspNetUsers", "HouseholdId", "dbo.Households", "Id", cascadeDelete: true);
        }
    }
}

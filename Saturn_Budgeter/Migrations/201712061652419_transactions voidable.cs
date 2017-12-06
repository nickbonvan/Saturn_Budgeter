namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transactionsvoidable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Void", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "Void");
        }
    }
}

namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class descriptioncolumninhousehold : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Households", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Households", "Description");
        }
    }
}

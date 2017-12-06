namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addnamestousers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "DisplayName", c => c.String(maxLength:128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "DisplayName");
        }
    }
}

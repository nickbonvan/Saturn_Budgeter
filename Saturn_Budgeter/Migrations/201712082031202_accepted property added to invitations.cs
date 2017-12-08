namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class acceptedpropertyaddedtoinvitations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invitations", "Accepted", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invitations", "Accepted");
        }
    }
}

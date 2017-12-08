namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invitationrecipientinfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invitations", "RecipientFirstName", c => c.String());
            AddColumn("dbo.Invitations", "RecipientLastName", c => c.String());
            AddColumn("dbo.Invitations", "RecipientEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invitations", "RecipientEmail");
            DropColumn("dbo.Invitations", "RecipientLastName");
            DropColumn("dbo.Invitations", "RecipientFirstName");
        }
    }
}

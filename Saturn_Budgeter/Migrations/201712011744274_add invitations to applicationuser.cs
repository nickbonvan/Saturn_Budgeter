namespace Saturn_Budgeter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addinvitationstoapplicationuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invitations", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Invitations", "ApplicationUser_Id");
            AddForeignKey("dbo.Invitations", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invitations", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Invitations", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Invitations", "ApplicationUser_Id");
        }
    }
}

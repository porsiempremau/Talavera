namespace TalaveraWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFullName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NombreCompleto", c => c.String(nullable: false, maxLength: 200));
            AddColumn("dbo.AspNetUsers", "Nick", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Nick");
            DropColumn("dbo.AspNetUsers", "NombreCompleto");
        }
    }
}

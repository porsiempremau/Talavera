namespace TalaveraWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class largoVariablesExtraUsuarios : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "NombreCompleto", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.AspNetUsers", "Nick", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Nick", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "NombreCompleto", c => c.String(nullable: false));
        }
    }
}

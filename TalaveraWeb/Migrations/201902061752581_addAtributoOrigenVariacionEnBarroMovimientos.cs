namespace TalaveraWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAtributoOrigenVariacionEnBarroMovimientos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BarroMovimientos", "OrigenVariacion", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BarroMovimientos", "OrigenVariacion");
        }
    }
}

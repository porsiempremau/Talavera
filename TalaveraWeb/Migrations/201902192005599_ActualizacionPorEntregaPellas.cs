namespace TalaveraWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizacionPorEntregaPellas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReservaBarroPreparadoes", "NumeroCarga", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReservaBarroPreparadoes", "NumeroCarga");
        }
    }
}

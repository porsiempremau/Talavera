namespace TalaveraWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterUnidadesYTotalBarroMovimientos : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BarroMovimientos", "Unidades", c => c.Double(nullable: false));
            AlterColumn("dbo.BarroMovimientos", "PesoTotal", c => c.Double());
            AlterColumn("dbo.ReservaBarroPreparadoes", "Capacidad", c => c.Double());
            AlterColumn("dbo.ReservaBarroPreparadoes", "Unidades", c => c.Double());
            AlterColumn("dbo.ReservaBarroPreparadoes", "TotalKg", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReservaBarroPreparadoes", "TotalKg", c => c.Int());
            AlterColumn("dbo.ReservaBarroPreparadoes", "Unidades", c => c.Int());
            AlterColumn("dbo.ReservaBarroPreparadoes", "Capacidad", c => c.Int());
            AlterColumn("dbo.BarroMovimientos", "PesoTotal", c => c.Int());
            AlterColumn("dbo.BarroMovimientos", "Unidades", c => c.Int(nullable: false));
        }
    }
}

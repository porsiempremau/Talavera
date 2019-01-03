namespace TalaveraWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCampoAEntregaPellas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntregaPellas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaMovimiento = c.DateTime(),
                        Responsable = c.String(),
                        TipoMovimiento = c.String(),
                        CantidadPellas = c.Int(),
                        NumCarga = c.String(),
                        Editor = c.String(),
                        FechaEdicion = c.DateTime(),
                        Locacion = c.Int(),
                        Observacion = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EntregaPellas");
        }
    }
}

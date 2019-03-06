namespace TalaveraWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPlanTrabajoYModelado : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MoldeadoMovimientos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdOrigen = c.Int(nullable: false),
                        TablaOrigen = c.String(),
                        IdCatalogoTalavera = c.Int(nullable: false),
                        CatidadPlaneada = c.Int(nullable: false),
                        CantidadReal = c.Int(nullable: false),
                        Observacion = c.String(),
                        TipoMovimiento = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlanDeTrabajoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdPersonal = c.Int(nullable: false),
                        NumeroOrden = c.String(maxLength: 15),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFin = c.DateTime(nullable: false),
                        EtapaPlan = c.String(maxLength: 15),
                        Observacion = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PlanDeTrabajoes");
            DropTable("dbo.MoldeadoMovimientos");
        }
    }
}

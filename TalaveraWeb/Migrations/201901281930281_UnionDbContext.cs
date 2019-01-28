namespace TalaveraWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnionDbContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BarroMaestra",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoProducto = c.String(maxLength: 5),
                        Capacidad = c.Int(),
                        Tipo = c.String(maxLength: 10),
                        Editor = c.String(maxLength: 50),
                        FechaEdicion = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BarroMovimientos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoProducto = c.String(maxLength: 5),
                        FechaMovimiento = c.DateTime(nullable: false, storeType: "date"),
                        TipoMovimiento = c.String(maxLength: 2),
                        Unidades = c.Int(nullable: false),
                        Locacion = c.Int(),
                        OrigenTransferencia = c.Int(),
                        OrigenTabla = c.String(maxLength: 20),
                        PesoTotal = c.Int(),
                        Editor = c.String(maxLength: 50),
                        FechaEdicion = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PreparacionBarro",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaPreparacion = c.DateTime(),
                        NumPreparado = c.String(maxLength: 10),
                        BarroNegro = c.Int(),
                        BarroBlanco = c.Int(),
                        Recuperado = c.Int(),
                        EnPiedra = c.String(maxLength: 10),
                        TiempoAgitacion = c.String(maxLength: 20),
                        NumTambos = c.Int(),
                        DesperdicioMojado = c.String(maxLength: 10),
                        Comentario = c.String(maxLength: 50),
                        Estado = c.String(maxLength: 10),
                        Locacion = c.Int(),
                        Editor = c.String(maxLength: 50),
                        FechaEdicion = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReservaBarroPreparadoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BarroUsado = c.Int(nullable: false),
                        CodigoBarro = c.String(),
                        Tipo = c.String(),
                        Capacidad = c.Int(),
                        Unidades = c.Int(),
                        TotalKg = c.Int(),
                        PreparacionBarroConsumo_Id = c.Int(),
                        PreparacionBarroConsumo_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PreparacionBarro", t => t.PreparacionBarroConsumo_Id)
                .ForeignKey("dbo.PreparacionBarro", t => t.PreparacionBarroConsumo_Id1)
                .Index(t => t.PreparacionBarroConsumo_Id)
                .Index(t => t.PreparacionBarroConsumo_Id1);
            
            CreateTable(
                "dbo.PreparacionPellas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fuente = c.Int(),
                        NumCarga = c.String(maxLength: 10),
                        FechaVaciado = c.DateTime(),
                        FechaLevantado = c.DateTime(),
                        FechaInicoPisado = c.DateTime(),
                        FechaFinPisado = c.DateTime(),
                        NumPeyas = c.Int(),
                        Restante = c.Int(),
                        CargaTotal = c.Int(),
                        Editor = c.String(maxLength: 50),
                        FechaEdicion = c.DateTime(),
                        Locacion = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.prepBarro_prepPellas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumCarga = c.String(maxLength: 10),
                        NumPreparado = c.String(maxLength: 10),
                        Editor = c.String(maxLength: 50),
                        FechaEdicion = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Provedores",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 250),
                        Telefono = c.String(maxLength: 15),
                        Telefono2 = c.String(maxLength: 15),
                        Numero = c.String(maxLength: 50),
                        Calle = c.String(maxLength: 50),
                        Colonia = c.String(maxLength: 50),
                        Municipio = c.String(maxLength: 50),
                        Estado = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Recuperados",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaRecuperado = c.DateTime(),
                        Responsable = c.String(maxLength: 50),
                        Cantidad = c.Int(),
                        NumRecuperado = c.String(maxLength: 10),
                        TipoMovimiento = c.String(maxLength: 1),
                        Editor = c.String(maxLength: 50),
                        FechaEdicion = c.DateTime(),
                        Locacion = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sucursales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 250),
                        Telefono = c.String(maxLength: 15),
                        Telefono2 = c.String(maxLength: 15),
                        Numero = c.String(maxLength: 50),
                        Calle = c.String(maxLength: 50),
                        Colonia = c.String(maxLength: 50),
                        Municipio = c.String(maxLength: 50),
                        Estado = c.String(maxLength: 50),
                        SiglaCodigo = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReservaBarroPreparadoes", "PreparacionBarroConsumo_Id1", "dbo.PreparacionBarro");
            DropForeignKey("dbo.ReservaBarroPreparadoes", "PreparacionBarroConsumo_Id", "dbo.PreparacionBarro");
            DropIndex("dbo.ReservaBarroPreparadoes", new[] { "PreparacionBarroConsumo_Id1" });
            DropIndex("dbo.ReservaBarroPreparadoes", new[] { "PreparacionBarroConsumo_Id" });
            DropTable("dbo.Sucursales");
            DropTable("dbo.Recuperados");
            DropTable("dbo.Provedores");
            DropTable("dbo.prepBarro_prepPellas");
            DropTable("dbo.PreparacionPellas");
            DropTable("dbo.ReservaBarroPreparadoes");
            DropTable("dbo.PreparacionBarro");
            DropTable("dbo.BarroMovimientos");
            DropTable("dbo.BarroMaestra");
        }
    }
}

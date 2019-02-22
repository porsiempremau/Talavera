namespace TalaveraWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCatalogoTalavera : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CatalogoTalaveras",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombrePieza = c.String(maxLength: 250),
                        Altura = c.Double(),
                        Diametro = c.Double(),
                        Tipo = c.String(),
                        Imagen = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CatalogoTalaveras");
        }
    }
}

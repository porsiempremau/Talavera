namespace TalaveraWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class camposObligatoriosCatalogoTalavera : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CatalogoTalaveras", "NombrePieza", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.CatalogoTalaveras", "Altura", c => c.Double(nullable: false));
            AlterColumn("dbo.CatalogoTalaveras", "Diametro", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CatalogoTalaveras", "Diametro", c => c.Double());
            AlterColumn("dbo.CatalogoTalaveras", "Altura", c => c.Double());
            AlterColumn("dbo.CatalogoTalaveras", "NombrePieza", c => c.String(maxLength: 250));
        }
    }
}

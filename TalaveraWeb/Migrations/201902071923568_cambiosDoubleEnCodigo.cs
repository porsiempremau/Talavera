namespace TalaveraWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambiosDoubleEnCodigo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PreparacionBarro", "BarroNegro", c => c.Double());
            AlterColumn("dbo.PreparacionBarro", "BarroBlanco", c => c.Double());
            AlterColumn("dbo.PreparacionBarro", "Recuperado", c => c.Double());
            AlterColumn("dbo.ReservaBarroPreparadoes", "BarroUsado", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReservaBarroPreparadoes", "BarroUsado", c => c.Int(nullable: false));
            AlterColumn("dbo.PreparacionBarro", "Recuperado", c => c.Int());
            AlterColumn("dbo.PreparacionBarro", "BarroBlanco", c => c.Int());
            AlterColumn("dbo.PreparacionBarro", "BarroNegro", c => c.Int());
        }
    }
}

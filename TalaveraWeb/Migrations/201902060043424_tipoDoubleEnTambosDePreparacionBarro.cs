namespace TalaveraWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tipoDoubleEnTambosDePreparacionBarro : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PreparacionBarro", "NumTambos", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PreparacionBarro", "NumTambos", c => c.Int());
        }
    }
}

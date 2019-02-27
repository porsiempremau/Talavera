namespace TalaveraWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTablaPersonalTalavera : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PersonalTalaveras",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 150),
                        APaterno = c.String(maxLength: 150),
                        AMaterno = c.String(maxLength: 150),
                        Puesto = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PersonalTalaveras");
        }
    }
}

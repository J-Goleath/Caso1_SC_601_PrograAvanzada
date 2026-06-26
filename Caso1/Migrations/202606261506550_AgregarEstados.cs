namespace Caso1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarEstados : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Estados",
                c => new
                    {
                        EstadoId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Descripcion = c.String(maxLength: 200),
                        Orden = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EstadoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Estados");
        }
    }
}

namespace Caso1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTareasTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tareas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 100),
                        Detalle = c.String(nullable: false, maxLength: 500),
                        FechaHora = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        Borrado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tareas");
        }
    }
}

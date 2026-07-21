namespace Caso1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarRolCompartidoATareas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tareas", "RolCompartido", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tareas", "RolCompartido");
        }
    }
}

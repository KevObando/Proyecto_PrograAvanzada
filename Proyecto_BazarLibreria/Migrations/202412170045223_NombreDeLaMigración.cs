namespace Proyecto_BazarLibreria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NombreDeLaMigración : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reseña", "ProductoCodigo", "dbo.Productoes");
            DropIndex("dbo.Reseña", new[] { "ProductoCodigo" });
            RenameColumn(table: "dbo.Reseña", name: "ProductoCodigo", newName: "Producto_Codigo");
            AddColumn("dbo.Reseña", "ProductoId", c => c.Int(nullable: false));
            AddColumn("dbo.Reseña", "Usuario", c => c.String());
            AddColumn("dbo.Reseña", "Calificación", c => c.Int(nullable: false));
            AlterColumn("dbo.Reseña", "Producto_Codigo", c => c.Int());
            CreateIndex("dbo.Reseña", "Producto_Codigo");
            AddForeignKey("dbo.Reseña", "Producto_Codigo", "dbo.Productoes", "Codigo");
            DropColumn("dbo.Reseña", "Calificacion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reseña", "Calificacion", c => c.Int(nullable: false));
            DropForeignKey("dbo.Reseña", "Producto_Codigo", "dbo.Productoes");
            DropIndex("dbo.Reseña", new[] { "Producto_Codigo" });
            AlterColumn("dbo.Reseña", "Producto_Codigo", c => c.Int(nullable: false));
            DropColumn("dbo.Reseña", "Calificación");
            DropColumn("dbo.Reseña", "Usuario");
            DropColumn("dbo.Reseña", "ProductoId");
            RenameColumn(table: "dbo.Reseña", name: "Producto_Codigo", newName: "ProductoCodigo");
            CreateIndex("dbo.Reseña", "ProductoCodigo");
            AddForeignKey("dbo.Reseña", "ProductoCodigo", "dbo.Productoes", "Codigo", cascadeDelete: true);
        }
    }
}

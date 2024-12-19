namespace Proyecto_BazarLibreria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Actualizaciones : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pedidoes", "HistorialId", "dbo.Historials");
            DropForeignKey("dbo.Historials", "UsuarioCodigo", "dbo.Usuarios");
            DropIndex("dbo.Historials", new[] { "UsuarioCodigo" });
            DropIndex("dbo.Pedidoes", new[] { "HistorialId" });
            RenameColumn(table: "dbo.Historials", name: "UsuarioCodigo", newName: "Usuario_Codigo");
            AddColumn("dbo.Reseña", "UsuarioId", c => c.String());
            AddColumn("dbo.Reseña", "Calificacion", c => c.Int(nullable: false));
            AddColumn("dbo.Historials", "UsuarioId", c => c.String());
            AddColumn("dbo.Pedidoes", "UsuarioId", c => c.String());
            AlterColumn("dbo.Historials", "Usuario_Codigo", c => c.Int());
            AlterColumn("dbo.Pedidoes", "HistorialId", c => c.Int());
            CreateIndex("dbo.Historials", "Usuario_Codigo");
            CreateIndex("dbo.Pedidoes", "HistorialId");
            AddForeignKey("dbo.Pedidoes", "HistorialId", "dbo.Historials", "Id");
            AddForeignKey("dbo.Historials", "Usuario_Codigo", "dbo.Usuarios", "Codigo");
            DropColumn("dbo.Reseña", "Usuario");
            DropColumn("dbo.Reseña", "Calificación");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reseña", "Calificación", c => c.Int(nullable: false));
            AddColumn("dbo.Reseña", "Usuario", c => c.String());
            DropForeignKey("dbo.Historials", "Usuario_Codigo", "dbo.Usuarios");
            DropForeignKey("dbo.Pedidoes", "HistorialId", "dbo.Historials");
            DropIndex("dbo.Pedidoes", new[] { "HistorialId" });
            DropIndex("dbo.Historials", new[] { "Usuario_Codigo" });
            AlterColumn("dbo.Pedidoes", "HistorialId", c => c.Int(nullable: false));
            AlterColumn("dbo.Historials", "Usuario_Codigo", c => c.Int(nullable: false));
            DropColumn("dbo.Pedidoes", "UsuarioId");
            DropColumn("dbo.Historials", "UsuarioId");
            DropColumn("dbo.Reseña", "Calificacion");
            DropColumn("dbo.Reseña", "UsuarioId");
            RenameColumn(table: "dbo.Historials", name: "Usuario_Codigo", newName: "UsuarioCodigo");
            CreateIndex("dbo.Pedidoes", "HistorialId");
            CreateIndex("dbo.Historials", "UsuarioCodigo");
            AddForeignKey("dbo.Historials", "UsuarioCodigo", "dbo.Usuarios", "Codigo", cascadeDelete: true);
            AddForeignKey("dbo.Pedidoes", "HistorialId", "dbo.Historials", "Id", cascadeDelete: true);
        }
    }
}

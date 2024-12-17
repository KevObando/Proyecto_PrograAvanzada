namespace Proyecto_BazarLibreria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarritoItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ProductoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        FechaAgregado = c.DateTime(nullable: false),
                        Producto_Codigo = c.Int(),
                        Usuario_Codigo = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Productoes", t => t.Producto_Codigo)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Codigo)
                .Index(t => t.Producto_Codigo)
                .Index(t => t.Usuario_Codigo);
            
            CreateTable(
                "dbo.Productoes",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Disponibilidad = c.Boolean(nullable: false),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo);
            
            CreateTable(
                "dbo.Imagens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        ProductoCodigo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Productoes", t => t.ProductoCodigo, cascadeDelete: true)
                .Index(t => t.ProductoCodigo);
            
            CreateTable(
                "dbo.Reseña",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductoId = c.Int(nullable: false),
                        Usuario = c.String(),
                        Comentario = c.String(),
                        Calificación = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Producto_Codigo = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Productoes", t => t.Producto_Codigo)
                .Index(t => t.Producto_Codigo);
            
            CreateTable(
                "dbo.Historials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioCodigo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioCodigo, cascadeDelete: true)
                .Index(t => t.UsuarioCodigo);
            
            CreateTable(
                "dbo.Pedidoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HistorialId = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Historials", t => t.HistorialId, cascadeDelete: true)
                .Index(t => t.HistorialId);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Contraseña = c.String(),
                        UltimaConexion = c.DateTime(nullable: false),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Historials", "UsuarioCodigo", "dbo.Usuarios");
            DropForeignKey("dbo.CarritoItems", "Usuario_Codigo", "dbo.Usuarios");
            DropForeignKey("dbo.Pedidoes", "HistorialId", "dbo.Historials");
            DropForeignKey("dbo.Reseña", "Producto_Codigo", "dbo.Productoes");
            DropForeignKey("dbo.Imagens", "ProductoCodigo", "dbo.Productoes");
            DropForeignKey("dbo.CarritoItems", "Producto_Codigo", "dbo.Productoes");
            DropIndex("dbo.Pedidoes", new[] { "HistorialId" });
            DropIndex("dbo.Historials", new[] { "UsuarioCodigo" });
            DropIndex("dbo.Reseña", new[] { "Producto_Codigo" });
            DropIndex("dbo.Imagens", new[] { "ProductoCodigo" });
            DropIndex("dbo.CarritoItems", new[] { "Usuario_Codigo" });
            DropIndex("dbo.CarritoItems", new[] { "Producto_Codigo" });
            DropTable("dbo.Usuarios");
            DropTable("dbo.Pedidoes");
            DropTable("dbo.Historials");
            DropTable("dbo.Reseña");
            DropTable("dbo.Imagens");
            DropTable("dbo.Productoes");
            DropTable("dbo.CarritoItems");
        }
    }
}

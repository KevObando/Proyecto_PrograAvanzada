
namespace Proyecto_BazarLibreria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCarritoItems : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Carritoes", newName: "CarritoItems");
            DropForeignKey("dbo.CarritoProductoes", "Carrito_Id", "dbo.Carritoes");
            DropForeignKey("dbo.CarritoProductoes", "ProductoCodigo", "dbo.Productoes");
            DropForeignKey("dbo.Carritoes", "UsuarioCodigo", "dbo.Usuarios");
            DropIndex("dbo.CarritoProductoes", new[] { "ProductoCodigo" });
            DropIndex("dbo.CarritoProductoes", new[] { "Carrito_Id" });
            DropIndex("dbo.CarritoItems", new[] { "UsuarioCodigo" });
            RenameColumn(table: "dbo.CarritoItems", name: "UsuarioCodigo", newName: "Usuario_Codigo");
            AddColumn("dbo.CarritoItems", "UserId", c => c.String());
            AddColumn("dbo.CarritoItems", "ProductoId", c => c.Int(nullable: false));
            AddColumn("dbo.CarritoItems", "Cantidad", c => c.Int(nullable: false));
            AddColumn("dbo.CarritoItems", "FechaAgregado", c => c.DateTime(nullable: false));
            AddColumn("dbo.CarritoItems", "Producto_Codigo", c => c.Int());
            AlterColumn("dbo.CarritoItems", "Usuario_Codigo", c => c.Int());
            CreateIndex("dbo.CarritoItems", "Producto_Codigo");
            CreateIndex("dbo.CarritoItems", "Usuario_Codigo");
            AddForeignKey("dbo.CarritoItems", "Producto_Codigo", "dbo.Productoes", "Codigo");
            AddForeignKey("dbo.CarritoItems", "Usuario_Codigo", "dbo.Usuarios", "Codigo");
            DropColumn("dbo.CarritoItems", "Total");
            DropTable("dbo.CarritoProductoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CarritoProductoes",
                c => new
                    {
                        CarritoId = c.Int(nullable: false, identity: true),
                        ProductoCodigo = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Carrito_Id = c.Int(),
                    })
                .PrimaryKey(t => t.CarritoId);
            
            AddColumn("dbo.CarritoItems", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.CarritoItems", "Usuario_Codigo", "dbo.Usuarios");
            DropForeignKey("dbo.CarritoItems", "Producto_Codigo", "dbo.Productoes");
            DropIndex("dbo.CarritoItems", new[] { "Usuario_Codigo" });
            DropIndex("dbo.CarritoItems", new[] { "Producto_Codigo" });
            AlterColumn("dbo.CarritoItems", "Usuario_Codigo", c => c.Int(nullable: false));
            DropColumn("dbo.CarritoItems", "Producto_Codigo");
            DropColumn("dbo.CarritoItems", "FechaAgregado");
            DropColumn("dbo.CarritoItems", "Cantidad");
            DropColumn("dbo.CarritoItems", "ProductoId");
            DropColumn("dbo.CarritoItems", "UserId");
            RenameColumn(table: "dbo.CarritoItems", name: "Usuario_Codigo", newName: "UsuarioCodigo");
            CreateIndex("dbo.CarritoItems", "UsuarioCodigo");
            CreateIndex("dbo.CarritoProductoes", "Carrito_Id");
            CreateIndex("dbo.CarritoProductoes", "ProductoCodigo");
            AddForeignKey("dbo.Carritoes", "UsuarioCodigo", "dbo.Usuarios", "Codigo", cascadeDelete: true);
            AddForeignKey("dbo.CarritoProductoes", "ProductoCodigo", "dbo.Productoes", "Codigo", cascadeDelete: true);
            AddForeignKey("dbo.CarritoProductoes", "Carrito_Id", "dbo.Carritoes", "Id");
            RenameTable(name: "dbo.CarritoItems", newName: "Carritoes");
        }
    }
}

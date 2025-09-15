using System.Collections.Generic;

namespace AngularApp.Server.Model
{
    public class ProductoStockItemDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal ValorInventario => Stock * PrecioVenta;
    }

    public class ReporteProductosDto
    {
        public int TotalProductos { get; set; }
        public int ProductosActivos { get; set; }
        public int ProductosInactivos { get; set; }
        public int StockTotalUnidades { get; set; }
        public decimal ValorTotalInventario { get; set; }
        public int BajoStockCantidad { get; set; }
        public List<ProductoStockItemDto> BajoStock { get; set; } = new();
        public List<ProductoStockItemDto> TopValorInventario { get; set; } = new();
    }
}

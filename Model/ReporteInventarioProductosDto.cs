namespace AngularApp.Server.Model
{
    public class ProductoStockBajoDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int Stock { get; set; }
        public int Umbral { get; set; }
        public decimal ValorInventario => Stock * PrecioCompra;
        public decimal PrecioCompra { get; set; }
    }

    public class ReporteInventarioProductosDto
    {
        public int TotalProductosActivos { get; set; }
        public int TotalItemsEnStock { get; set; }
        public decimal ValorTotalCosto { get; set; }
        public decimal ValorTotalVenta { get; set; }
        public decimal MargenPotencial => ValorTotalVenta - ValorTotalCosto;
        public List<ProductoStockBajoDto> ProductosStockBajo { get; set; } = new();
        public DateTime GeneradoEnUtc { get; set; } = DateTime.UtcNow;
        public int UmbralStockBajo { get; set; }
    }
}

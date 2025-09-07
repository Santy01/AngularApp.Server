namespace AngularApp.Server.Model
{
    public class ProductoModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty; // SKU o código único
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; } = true;
    }
}

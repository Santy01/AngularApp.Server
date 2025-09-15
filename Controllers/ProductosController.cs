using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularApp.Server.Data;
using AngularApp.Server.Model;

namespace AngularApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ServerDbContext _context;
        public ProductosController(ServerDbContext context)
        {
            _context = context;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoModel>>> GetProductos()
        {
            return await _context.Productos.AsNoTracking().ToListAsync();
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoModel>> GetProducto(int id)
        {
            var prod = await _context.Productos.FindAsync(id);
            if (prod == null) return NotFound();
            return prod;
        }

        // POST: api/Productos
        [HttpPost]
        public async Task<ActionResult<ProductoModel>> PostProducto(ProductoModel producto)
        {
            // Código único
            if (await _context.Productos.AnyAsync(p => p.Codigo == producto.Codigo))
            {
                return Conflict($"El código {producto.Codigo} ya existe");
            }
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        // PUT: api/Productos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, ProductoModel producto)
        {
            if (id != producto.Id) return BadRequest();
            // Validar código duplicado (excluyendo el propio)
            if (await _context.Productos.AnyAsync(p => p.Codigo == producto.Codigo && p.Id != id))
            {
                return Conflict($"El código {producto.Codigo} ya está en uso por otro producto");
            }
            _context.Entry(producto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id)) return NotFound(); else throw;
            }
            return Ok(producto);
        }

        // PATCH: api/Productos/5/stock?cantidad=3 (ajuste simple de stock)
        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> AjustarStock(int id, [FromQuery] int cantidad)
        {
            var prod = await _context.Productos.FindAsync(id);
            if (prod == null) return NotFound();
            prod.Stock += cantidad; // cantidad puede ser negativa
            await _context.SaveChangesAsync();
            return Ok(prod.Stock);
        }

        // GET: api/Productos/reporte-inventario
        [HttpGet("reporte-inventario")]
        public async Task<ActionResult<ReporteInventarioProductosDto>> GetReporteInventarioProductos([FromQuery] int umbralStockBajo = 5)
        {
            if (umbralStockBajo < 0) umbralStockBajo = 0;
            var lista = await _context.Productos.AsNoTracking().Where(p => p.Activo).ToListAsync();

            var dto = new ReporteInventarioProductosDto
            {
                TotalProductosActivos = lista.Count,
                TotalItemsEnStock = lista.Sum(p => p.Stock),
                ValorTotalCosto = lista.Sum(p => p.Stock * p.PrecioCompra),
                ValorTotalVenta = lista.Sum(p => p.Stock * p.PrecioVenta),
                UmbralStockBajo = umbralStockBajo,
                ProductosStockBajo = lista.Where(p => p.Stock <= umbralStockBajo)
                    .OrderBy(p => p.Stock)
                    .Select(p => new ProductoStockBajoDto
                    {
                        Id = p.Id,
                        Codigo = p.Codigo,
                        Nombre = p.Nombre,
                        Stock = p.Stock,
                        Umbral = umbralStockBajo,
                        PrecioCompra = p.PrecioCompra
                    }).ToList()
            };
            return Ok(dto);
        }

        // DELETE: api/Productos/5 (borrado lógico)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var prod = await _context.Productos.FindAsync(id);
            if (prod == null) return NotFound();
            _context.Productos.Remove(prod);
            await _context.SaveChangesAsync();
            return Ok(id);
        }

        private bool ProductoExists(int id) => _context.Productos.Any(p => p.Id == id);
    }
}

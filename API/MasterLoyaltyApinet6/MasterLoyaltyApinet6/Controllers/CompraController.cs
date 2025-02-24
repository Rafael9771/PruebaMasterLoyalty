using MasterLoyaltyApinet6.Data;
using MasterLoyaltyApinet6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MasterLoyaltyApinet6.Controllers;

[ApiController]
[Route("api/compra")]
public class CompraController : Controller
{
    private readonly AppDbContext context;
    private readonly IConfiguration _config;

    public CompraController(AppDbContext context, IConfiguration config)
    {
        this.context = context;
        _config = config;
    }

    [HttpPost("CreateCompra")]
    [Authorize]
    public async Task<IActionResult> Comprar()
    {

        
        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {

            int clienteId = int.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value);

            if (clienteId == null)
            {
                return new JsonResult(
                    new
                    {
                        message = HttpStatusCode.BadRequest.ToString(),
                        code = HttpStatusCode.BadRequest,
                        response = "Usuario invalido"
                    });
            }
            // Obtener los carritos activos del cliente
            var carritos = await context.Carritos
                .Where(c => c.CaClId == clienteId && c.CaStatus == 1)
                .ToListAsync();

            if (!carritos.Any())
            {
                return new JsonResult(
                    new
                    {
                        message = HttpStatusCode.NotFound.ToString(),
                        code = HttpStatusCode.NotFound,
                        response = "No hay artículos en el carrito para comprar."
                    });
                
            }

            List<Compra> compras = new List<Compra>();

            foreach (var carrito in carritos)
            {
                // Obtener la relación TiendaArticulo
                var tiendaArticulo = await context.TiendaArticulos
                    .FirstOrDefaultAsync(t => t.TiarId == carrito.CaArTiId);

                if (tiendaArticulo == null)
                {
                    return new JsonResult(
                    new
                    {
                        message = HttpStatusCode.BadRequest.ToString(),
                        code = HttpStatusCode.BadRequest,
                        response = $"El artículo con ID {carrito.CaArTiId} no existe en la tienda."
                    });
                }

                if (tiendaArticulo.TiarStockTienda < carrito.CaCantidad)
                {
                    return new JsonResult(
                    new
                    {
                        message = HttpStatusCode.BadRequest.ToString(),
                        code = HttpStatusCode.BadRequest,
                        response = $"Stock insuficiente para el artículo con ID {carrito.CaArTiId}."
                    });
                }

                // Restar stock
                tiendaArticulo.TiarStockTienda -= carrito.CaCantidad;

                // Marcar carrito como comprado (status = 2)
                carrito.CaStatus = 2;

                // Crear folio (Ejemplo: CO-20240224-XXXXX)
                string folio = $"CO-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 5).ToUpper()}";

                // Crear objeto de compra
                var compra = new Compra
                {
                    CoGuid = Guid.NewGuid(),
                    CoClId = carrito.CaClId,
                    CoTiId = tiendaArticulo.TiarTiId,
                    CoArId = tiendaArticulo.TiarArId,
                    CoFolio = folio,
                    CoCantidad = carrito.CaCantidad,
                    CoDcreate = DateTime.Now,
                    CoStatus = 1
                };

                compras.Add(compra);
            }

            // Agregar compras a la base de datos
            await context.Compras.AddRangeAsync(compras);

            // Guardar cambios en la BD
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new JsonResult(
                    new
                    {
                        message = HttpStatusCode.OK.ToString(),
                        code = HttpStatusCode.OK,
                        response = "Compra realizada con éxito"
                    });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new JsonResult(
                    new
                    {
                        message = HttpStatusCode.BadRequest.ToString(),
                        code = HttpStatusCode.BadRequest,
                        response = "Error al procesar la compra"
                    });
        }
    }

}


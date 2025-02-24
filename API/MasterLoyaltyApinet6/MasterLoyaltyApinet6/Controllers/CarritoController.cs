using apiMasterLoyalty.Request;
using apiMasterLoyalty.Response;
using EFCore.BulkExtensions;
using MasterLoyaltyApinet6.Data;
using MasterLoyaltyApinet6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace apiMasterLoyalty.Controllers;


[ApiController]
[Route("api/carrito")]
public class CarritoController : ControllerBase
{
    private readonly AppDbContext context;
    private readonly IConfiguration _config;

    public CarritoController(AppDbContext context, IConfiguration config)
    {
        this.context = context;
        _config = config;
    }

    [HttpGet]
    [Route("GetMyShopsCart")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<responseGetMyCart>>> GetMyShopsCart()
    {
        var authenticated = this.User.Identity.IsAuthenticated;
        var uservar = User;
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        var userId = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

        //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // "sub" en el JWT
        var username = User.FindFirst(ClaimTypes.Name)?.Value; // Nombre del usuario
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList(); // Lista de roles

        

        if (userId == null)
        {
            return new JsonResult(
                new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = "Usuario invalido"
                });
        }

        if (!context.Carritos.Any(c => c.CaClId == int.Parse(userId) && c.CaStatus == 1))
        {
            return new JsonResult(
                new
                {
                    message = HttpStatusCode.NotFound.ToString(),
                    code = HttpStatusCode.NotFound,
                    response = "Este usuario no tiene un carrito activo"
                });
        }

        var result = await (from ca in context.Carritos
                            join tiar in context.TiendaArticulos on ca.CaArTiId equals tiar.TiarId
                            join ti in context.Tienda on tiar.TiarTiId equals ti.TiId
                            join ar in context.Articulos on tiar.TiarArId equals ar.ArId
                            where ca.CaClId == int.Parse(userId) && ca.CaStatus == 1
                            select new responseGetMyCart
                            {
                                CarritoId = ca.CaId,
                                TiendaId = ti.TiId,
                                Sucursal = ti.TiSucursal,
                                Articulo = ar.ArNombre,
                                Codigo = ar.ArCodigo,
                                Imagen = ar.ArImagen,
                                Cantidad = ca.CaCantidad
                            }).ToListAsync();

        return new JsonResult(
                new
                {
                    message = HttpStatusCode.OK.ToString(),
                    code = HttpStatusCode.OK,
                    response = result
                });

    }


    [HttpPost]
    [Route("AddArticleToShopCart")]
    [Authorize]
    public async Task<ActionResult> AddArticleToShopCart(AddArticleToShopCartReq request)
    {

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userId == null) {
            return new JsonResult(
                new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = "Usuario invalido"
                });

        }
        else if (!context.TiendaArticulos.Any(a => a.TiarId == request.ArticuloTiendaId))
        {
            return new JsonResult(
                new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = "articulo inexistente"
                });

        }
        else if (context.TiendaArticulos.Any(a => a.TiarId == request.ArticuloTiendaId && (a.TiarStockTienda<1 || a.TiarStockTienda<request.Cantidad))) 
        {
            return new JsonResult(
                new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = "No hay suficiente stock del producto"
                });

        }else if (context.Carritos.Any(c => c.CaArTiId == request.ArticuloTiendaId))
        {
            var car = context.Carritos.Where(c => c.CaArTiId == request.ArticuloTiendaId).First();
            car.CaCantidad = request.Cantidad;

            context.Update(car);
            await context.SaveChangesAsync();

            return new JsonResult(
                new
                {
                    message = HttpStatusCode.OK.ToString(),
                    code = HttpStatusCode.OK,
                    response = "Producto agregado con exito"
                });
        }


        var newCarrito = new Carrito
        {
            CaClId=int.Parse(userId),
            CaCantidad=request.Cantidad,
            CaArTiId=request.ArticuloTiendaId
        };

        
        
        context.Add(newCarrito);

        await context.SaveChangesAsync();

        return new JsonResult(
                new
                {
                    message = HttpStatusCode.OK.ToString(),
                    code = HttpStatusCode.OK,
                    response = "Producto agregado con exito"
                });


    }

    [HttpDelete]
    [Route("ClearShopCart")]
    [Authorize]

    public async Task<ActionResult> ClearShopCart()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return new JsonResult(
                new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = "Usuario invalido"
                });

        }else if (!context.Carritos.Any(c=>c.CaClId==int.Parse(userId) && c.CaStatus==1))
        {
            return new JsonResult(
                new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = "El usuario no tiene ningun carrito activo para eliminar"
                });
        }

        //context.Carritos.Where(c => c.CaClId == int.Parse(userId) && c.CaStatus == 1).ExecuteUpdate(setters => setters.SetProperty(e => e.CaStatus, 0));

        var carritos = context.Carritos.Where(c=>c.CaClId == int.Parse(userId) && c.CaStatus==1).ToList();

        carritos.ForEach(c => {c.CaStatus = 0;});

        await context.BulkUpdateAsync(carritos);


        return new JsonResult(
                new
                {
                    message = HttpStatusCode.OK.ToString(),
                    code = HttpStatusCode.OK,
                    response = "Carrito eliminado correctamente"
                });
    }
}

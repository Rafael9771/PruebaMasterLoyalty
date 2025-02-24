using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MasterLoyaltyApinet6.Models;
using MasterLoyaltyApinet6.Data;
using apiMasterLoyalty.Request;
using apiMasterLoyalty.Response;


namespace ApiExpedienteMedico.Controllers;


[ApiController]
[Route("api/tienda")]
public class TiendaController : ControllerBase
{
    private readonly AppDbContext context;
    private readonly IConfiguration _config;

    public TiendaController(AppDbContext context, IConfiguration config)
    {
        this.context = context;
        _config = config;
    }

    [HttpGet]
    //Get para obtener los usuarios activos en la BD
    public async Task<ActionResult<List<Tiendum>>> GetShops()
    {
        try
        {
            var estados = await context.Tienda.Where(s => s.TiStatus == 1).ToListAsync();

            return new JsonResult(
            new
            {
                message = HttpStatusCode.OK.ToString(),
                code = HttpStatusCode.OK,
                response = estados
            });
        }
        catch (Exception ex)
        {
            var jOb = new
            {
                message = HttpStatusCode.BadRequest.ToString(),
                code = HttpStatusCode.BadRequest,
                response = ex.Message.ToString()
            };

            return BadRequest(jOb);
        }

    }

    
    [HttpPost]
    [Route("CreateShop")]
    //Endpoint para crear un nuevo usuario
    public async Task<ActionResult> CreateShop(createTiendaReq req)
    {
        try
        {
            if (context.Tienda.Any(u => u.TiSucursal == req.Sucursal))
            {
                var jOb = new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = "La sucursal '"+req.Sucursal+"' ya fue creada"
                };

                return BadRequest(jOb);

            }
            else
            {
                


                var tienda = new Tiendum
                {
                    TiSucursal = req.Sucursal,
                    TiDireccion = req.Direccion


                };

                context.Add(tienda);

                await context.SaveChangesAsync();



                return new JsonResult(
               new
               {
                   message = HttpStatusCode.OK.ToString(),
                   code = HttpStatusCode.OK,
                   response = true
               });

            }
        }
        catch (Exception ex)
        {
            var jOb = new
            {
                message = HttpStatusCode.BadRequest.ToString(),
                code = HttpStatusCode.BadRequest,
                response = ex.Message.ToString()
            };

            return BadRequest(jOb);
        }
    }

    [HttpDelete]
    [Route("DeleteShop")]
    public async Task<ActionResult> DeleteShop(Guid req)
    {
        try
        {
            if(context.Tienda.Any(n => n.TiGuid == req && n.TiStatus == 0))
            {
                var jOb = new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = "La sucursal ya estaba elliminado"
                };

                return BadRequest(jOb);
            }
            else
            {
                var tien = context.Tienda.Where(n=>n.TiGuid == req).First();

                tien.TiStatus = 0;

                context.Update(tien);
                await context.SaveChangesAsync();

                return new JsonResult(
               new
               {
                   message = HttpStatusCode.OK.ToString(),
                   code = HttpStatusCode.OK,
                   response = true
               });
            }
        }
        catch (Exception ex)
        {
            var jOb = new
            {
                message = HttpStatusCode.BadRequest.ToString(),
                code = HttpStatusCode.BadRequest,
                response = ex.Message.ToString()
            };

            return BadRequest(jOb);
        }
    }

    [HttpPut]
    [Route("UpdateShop")]
    public async Task<ActionResult> UpdateShop(Guid guid, createTiendaReq req)
    {
        try
        {
            if (!context.Tienda.Any(n => n.TiGuid == guid))
            {
                var jOb = new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = "La sucursal que intentas actualizar no existe"
                };

                return BadRequest(jOb);
            }
            else
            {
                var tien = context.Tienda.Where(n => n.TiGuid == guid).First();

                tien.TiStatus = 1;
                tien.TiSucursal = req.Sucursal;
                tien.TiDireccion = req.Direccion;
               

                context.Update(tien);
                await context.SaveChangesAsync();

                return new JsonResult(
               new
               {
                   message = HttpStatusCode.OK.ToString(),
                   code = HttpStatusCode.OK,
                   response = true
               });
            }
        }
        catch (Exception ex)
        {
            var jOb = new
            {
                message = HttpStatusCode.BadRequest.ToString(),
                code = HttpStatusCode.BadRequest,
                response = ex.Message.ToString()
            };

            return BadRequest(jOb);
        }
    }

    [HttpPost]
    [Route("AddArticleinShop")]
    //Endpoint para crear un nuevo usuario
    public async Task<ActionResult> AddArticleinShop(Guid tiendaGuid, int Cantidad, Guid articuloGuid)
    {
        if (!context.Tienda.Any(n => n.TiGuid == tiendaGuid))
        {
            var jOb = new
            {
                message = HttpStatusCode.BadRequest.ToString(),
                code = HttpStatusCode.BadRequest,
                response = "La sucursal no existe"
            };

            return BadRequest(jOb);
        }else if (!context.Articulos.Any(n=>n.ArGuid==articuloGuid))
        {
            var jOb = new
            {
                message = HttpStatusCode.BadRequest.ToString(),
                code = HttpStatusCode.BadRequest,
                response = "El articulo no existe"
            };

            return BadRequest(jOb);
        }
        else
        {
            var tien = context.Tienda.Where(t=>t.TiGuid==tiendaGuid).First();
            var art = context.Articulos.Where(a=>a.ArGuid==articuloGuid).First();

           
                var tienArt = new TiendaArticulo
                {
                    TiarArId = art.ArId,
                    TiarTiId = tien.TiId,
                    TiarStockTienda = Cantidad
                    

                };


                context.Add(tienArt);

                await context.SaveChangesAsync();

               return new JsonResult(
               new
               {
                   message = HttpStatusCode.OK.ToString(),
                   code = HttpStatusCode.OK,
                   response = true
               });

            
        }



        
    }
    //endpoint para obtener articulos y stock por guid de tienda
    [HttpGet]
    [Route("articulosTienda")]
    //Get para obtener los usuarios activos en la BD
    public async Task<ActionResult> GetArticles(Guid tiendaGuid)
    {
        try
        {
            var tien = await context.Tienda.Where(s => s.TiStatus == 1 && s.TiGuid == tiendaGuid).FirstAsync();

            if (tien == null)
            {
                return NotFound(new
                {
                    message = "Tienda no encontrada",
                    code = HttpStatusCode.NotFound,
                    response = "No se encontró la tienda con el GUID proporcionado."
                });
            }
            // Obtener los artículos de la tienda junto con su stock
            var articulos = await (from ta in context.TiendaArticulos
                             join a in context.Articulos
                                 on ta.TiarArId equals a.ArId  // Relación entre TiendaArticulo y Articulo por ArId
                             where ta.TiarTiId == tien.TiId && ta.TiarStockTienda>0 // Relacionamos con la tienda
                             select new articulosTienda
                             {
                                 articulo = a,                   // Artículo completo
                                 Stock = ta.TiarStockTienda,      // Stock en la tienda
                                 articuloTiendaId = ta.TiarId
                             }).ToListAsync();


            var responseTA = new responseArticlesTienda
            {
                articulos = articulos,
                Tienda = tien
            };

            return new JsonResult(
            new
            {
                message = HttpStatusCode.OK.ToString(),
                code = HttpStatusCode.OK,
                response = responseTA
            });
        }
        catch (Exception ex)
        {
            var jOb = new
            {
                message = HttpStatusCode.BadRequest.ToString(),
                code = HttpStatusCode.BadRequest,
                response = ex.Message.ToString()
            };

            return BadRequest(jOb);
        }

    }
}
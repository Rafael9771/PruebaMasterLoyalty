using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using apiMasterLoyalty.Models;
using apiMasterLoyalty.Data;
using apiMasterLoyalty.Request;


namespace ApiExpedienteMedico.Controllers;


[ApiController]
[Route("api/articulo")]
public class ArticuloController : ControllerBase
{
    private readonly AppDbContext context;
    private readonly IConfiguration _config;

    public ArticuloController(AppDbContext context, IConfiguration config)
    {
        this.context = context;
        _config = config;
    }

    [HttpGet]
    //Get para obtener los usuarios activos en la BD
    public async Task<ActionResult<List<Articulo>>> Get()
    {
        try
        {
            var estados = await context.Articulos.Where(s => s.ArStatus == 1).ToListAsync();

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
    [Route("create")]
    //Endpoint para crear un nuevo usuario
    public async Task<ActionResult> Post(createArticuloReq req)
    {
        try
        {
            if (context.Articulos.Any(u => u.ArNombre == req.Nombre))
            {
                var jOb = new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = "El articulo'"+req.Nombre+"' ya fue creado"
                };

                return BadRequest(jOb);

            }
            else
            {
                


                var article = new Articulo
                {
                    ArNombre = req.Nombre,
                    ArCodigo = req.Codigo,
                    ArStock = req.Stock,
                    ArDescripcion = req.Descripcion,
                    ArPrecio = req.Precio,
                    ArImagen = req.Imagen


                };

                context.Add(article);

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
    [Route("Delete")]
    public async Task<ActionResult> DeleteArticle(Guid req)
    {
        try
        {
            if(context.Articulos.Any(n => n.ArGuid == req && n.ArStatus == 0))
            {
                var jOb = new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = "El articulo ya estaba elliminado"
                };

                return BadRequest(jOb);
            }
            else
            {
                var art = context.Articulos.Where(n=>n.ArGuid == req).First();

                art.ArStatus = 0;

                context.Update(art);
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
    [Route("Update")]
    public async Task<ActionResult> UpdateArticle(Guid guid, createArticuloReq req)
    {
        try
        {
            if (!context.Articulos.Any(n => n.ArGuid == guid))
            {
                var jOb = new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = "El articulo que intentas actualizar no existe"
                };

                return BadRequest(jOb);
            }
            else
            {
                var art = context.Articulos.Where(n => n.ArGuid == guid).First();

                art.ArStatus = 1;
                art.ArNombre = req.Nombre;
                art.ArDescripcion = req.Descripcion;
                art.ArPrecio = req.Precio;
                art.ArStock = req.Stock;
                art.ArCodigo = req.Codigo;
                art.ArImagen = req.Imagen;

                context.Update(art);
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
}
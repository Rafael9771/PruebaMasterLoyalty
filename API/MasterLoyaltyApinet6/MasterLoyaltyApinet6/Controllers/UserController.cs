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
using Microsoft.AspNetCore.Authorization;


namespace ApiExpedienteMedico.Controllers;


    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IConfiguration _config;

        public UserController(AppDbContext context, IConfiguration config)
        {
            this.context = context;
            _config = config;
        }

        [HttpGet]
    
        //Get para obtener los usuarios activos en la BD
        public async Task<ActionResult<List<Cliente>>> Get()
        {
            try
            {
            var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            Console.WriteLine($"Nueva clave: {key}");
            var estados = await context.Clientes.Where(s => s.CiStatus == 1).ToListAsync();

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
        public async Task<ActionResult> Post(Cliente usuario)
        {
            try
            {
                if (context.Clientes.Any(u => u.CiCorreo == usuario.CiCorreo))
                {
                    var jOb = new
                    {
                        message = HttpStatusCode.BadRequest.ToString(),
                        code = HttpStatusCode.BadRequest,
                        response = "Existe un usuario con ese correo registrado"
                    };

                    return BadRequest(jOb);

                }
                else
                {
                    var content = usuario.CiPassword.ToString();
                    var key = "E546C8DF278CD5931069B522E695D4F2";

                    var encrypted = EncryptString(content, key);


                        var user = new Cliente
                        {
                            CiPassword = encrypted,
                            CiCorreo = usuario.CiCorreo,
                            CiNombre = usuario.CiNombre,
                            CiPrimerApellido = usuario.CiPrimerApellido,
                            CiSegundoApellido = usuario.CiSegundoApellido
                           
                            
                        };

                        context.Add(user);

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
        /*
        //Endpoint para crear codigo de reseteo de password
        [HttpPost]
        [Route("sendCode")]
        //Endpoint para inicio de sesion
        public async Task<ActionResult> sendCode(string mail)
        {
            try
            {
                if(context.Usuarios.Any(u=>u.UserEmail== mail))
                {
                    var user = context.Usuarios.Where(u => u.UserEmail == mail).First();
                    Random random = new Random();
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    string code = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());

                    user.UserCodeReset= code;
                    context.Update(user);
                    context.SaveChanges();
                    string body = "<p>Bienvenido a expendiete medico " + user.UserNombre + "</p><p>Se genero el siguiente codigo para cambiar su contraseña: <strong>"+code+"</strong></p>";
                    EnvioActivacion correoA = new();

                    if (await correoA.enviarCorreo(user.UserEmail, "Cambio de contraseña", body, false))
                    {

                        return new JsonResult(
                                   new
                                   {
                                       message = HttpStatusCode.OK.ToString(),
                                       code = HttpStatusCode.OK,
                                       response = "Se envio un codigo al correo:'"+mail+"' para cambiar su contraseña."
                                   });
                    }
                    else {
                        return new JsonResult(
                                   new
                                   {
                                       message = HttpStatusCode.BadRequest.ToString(),
                                       code = HttpStatusCode.BadRequest,
                                       response = "Error intente de nuevo mas tarde o contacte al administrador"
                                   });
                    }
                }
                else
                {
                    return new JsonResult(
                                   new
                                   {
                                       message = HttpStatusCode.NotFound.ToString(),
                                       code = HttpStatusCode.NotFound,
                                       response = "Correo no registrado"
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


        //Endpoint para cambiar el password si elcodigo coincide
        [HttpPost]
        [Route("resetPass")]
        //Endpoint para inicio de sesion
        public async Task<ActionResult> resetPass(string mail, string code, string newPass)
        {
            try
            {
                if (context.Usuarios.Any(u => u.UserEmail == mail))
                {
                    var user = context.Usuarios.Where(u => u.UserEmail == mail).First();

                    if (user.UserCodeReset == code)
                    {

                        var key = "E546C8DF278CD5931069B522E695D4F2";
                        var encrypted = EncryptString(newPass, key);

                        user.UserPass= encrypted;
                        user.UserCodeReset= "";
                        context.Update(user);
                        context.SaveChanges();


                        return new JsonResult(
                                new
                                {
                                    message = HttpStatusCode.OK.ToString(),
                                    code = HttpStatusCode.OK,
                                    response = "Se modifico la contraseña correctamente."
                                });
                    }
                    else
                    {
                        return new JsonResult(
                                new
                                {
                                    message = HttpStatusCode.BadRequest.ToString(),
                                    code = HttpStatusCode.BadRequest,
                                    response = "Error el codigo no coincide"
                                });
                    }

                    
                    
                      
                }
                else
                {
                    return new JsonResult(
                                   new
                                   {
                                       message = HttpStatusCode.NotFound.ToString(),
                                       code = HttpStatusCode.NotFound,
                                       response = "Correo no registrado"
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
        */

        [HttpPost]
        [Route("login")]
        //Endpoint para inicio de sesion
        public async Task<ActionResult> PostLogin(string mail, string pass)
        {
            try
            {
                var content = pass;
                var key = "E546C8DF278CD5931069B522E695D4F2";


                var userlogin = context.Clientes.Where(s => s.CiCorreo == mail).First();


                //bool result = context.Usuarios.Any(s => s.UserEmail == mail && DecryptString(s.UserPass, key) == pass);
                //Revision de password ingresado desincriptado
                if (DecryptString(userlogin.CiPassword.ToString(), key) == pass)
                {
                    if (userlogin.CiStatus == 0)
                    {

                        
                        return new JsonResult(
                            new
                            {
                                message = HttpStatusCode.BadRequest.ToString(),
                                code = HttpStatusCode.BadRequest,
                                response = "Tu cuenta no esta activada, porfavor revisa tu correo para activarla."
                            });
                        
                    }
                    else
                    {


                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, userlogin.CiNombre),
                            new Claim(ClaimTypes.GivenName, userlogin.CiPrimerApellido),
                            new Claim(ClaimTypes.Surname, userlogin.CiSegundoApellido),
                            new Claim(ClaimTypes.Email, userlogin.CiCorreo),
                            new Claim(ClaimTypes.Role, userlogin.CiRol),
                            new Claim(JwtRegisteredClaimNames.Sub, userlogin.CiId.ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                        };
                    

                    
                    var token = new JwtSecurityToken(
                            _config["Jwt:Issuer"],
                        _config["Jwt:Audience"],
                            claims, 
                            expires: DateTime.Now.AddMinutes(1440),
                            signingCredentials: credentials
                        );
                        return new JsonResult(
                           new
                           {
                               message = HttpStatusCode.OK.ToString(),
                               code = HttpStatusCode.OK,
                               response = new JwtSecurityTokenHandler().WriteToken(token)
                           });
                    }
                }
                else
                {
                    return new JsonResult(
                       new
                       {
                           message = HttpStatusCode.BadRequest.ToString(),
                           code = HttpStatusCode.BadRequest,
                           response = "mail o contraseña incorrectos"
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
    [Route("login2")]
    //Endpoint para inicio de sesion
    public async Task<ActionResult> PostLogin2()
    {
        try
        {




            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
                {
                    new Claim(ClaimTypes.Name, "Administrador"),
                    new Claim(ClaimTypes.Email, "admin@mail.com"),
                    new Claim(ClaimTypes.Role, "2"),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                        };



            var token = new JwtSecurityToken(
                            _config["Jwt:Issuer"],
                        _config["Jwt:Audience"],
                            claims,
                            expires: DateTime.Now.AddMinutes(1440),
                            signingCredentials: credentials
                        );
            return new JsonResult(
               new
               {
                   message = HttpStatusCode.OK.ToString(),
                   code = HttpStatusCode.OK,
                   response = new JwtSecurityTokenHandler().WriteToken(token)
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
    /*
    [HttpPost]
    [Route("createTeamsUser")]
    //Endpoint para crear un nuevo usuario de teams
    public async Task<ActionResult> createTeamsUser(Usuario usuario)
    {
        try
        {
            if (context.Usuarios.Any(u => u.UserEmail == usuario.UserEmail))
            {
                var jOb = new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = "Existe un usuario con ese correo registrado"
                };

                return BadRequest(jOb);

            }
            else
            {
                var content = usuario.UserPass.ToString();
                var key = "E546C8DF278CD5931069B522E695D4F2";

                var encrypted = EncryptString(content, key);

                EnvioActivacion correoA = new();

                Guid tokenAct = Guid.NewGuid();

                while (context.Usuarios.Any(u => u.UserTokenActivation == tokenAct))
                {
                    tokenAct = Guid.NewGuid();
                }


                string body = "<p>Bienvenido a expendiete medico " + usuario.UserNombre + " para activar tu cuenta porfavor da click en el siguiente enlace:</p><br><a href='" + apiurl.getapiURL() + "/api/users/activate?token=" + tokenAct.ToString() + "'>Activar cuenta</a>";





                    var user = new Usuario
                    {
                        UserNext = usuario.UserNext,
                        UserAge = usuario.UserAge,
                        UserCdatos = usuario.UserCdatos,
                        UserEmail = usuario.UserEmail,
                        UserCpId = usuario.UserCpId,
                        UserDcreate = usuario.UserDcreate,
                        UserCtId = usuario.UserCtId,
                        UserGenderId = usuario.UserGenderId,                            
                        UserNint = usuario.UserNint,
                        UserNombre = usuario.UserNombre,
                        UserPApellido = usuario.UserPApellido,
                        UserPass = encrypted.ToString(),
                        UserPhone = usuario.UserPhone,
                        UserRolId = 1,
                        UserSApellido = usuario.UserSApellido,
                        UserStatus = 1,
                        UserStreet = usuario.UserStreet,
                        UserTokenActivation = tokenAct,
                        UserNoEmpleado = usuario.UserNoEmpleado,
                        UserExpToken = DateTime.Now.AddMinutes(1440)

                    };

                    context.Add(user);

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
    [Route("loginTeamsUser")]
    //Endpoint para inicio de sesion
    public async Task<ActionResult> loginTeamsUser(string mail)
    {
        try
        {

            var key = "E546C8DF278CD5931069B522E695D4F2";





            //bool result = context.Usuarios.Any(s => s.UserEmail == mail && DecryptString(s.UserPass, key) == pass);
            //Revision de password ingresado desincriptado
            if (context.Usuarios.Any(s => s.UserEmail == mail))
            {

                var userlogin = context.Usuarios.Where(s => s.UserEmail == mail).First();





                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, userlogin.UserNombre),
                        new Claim(ClaimTypes.GivenName, userlogin.UserPApellido),
                        new Claim(ClaimTypes.Surname, userlogin.UserSApellido),
                        new Claim(ClaimTypes.Email, userlogin.UserEmail),
                        new Claim(ClaimTypes.Role, userlogin.UserRolId.ToString()),
                    };

                    var token = new JwtSecurityToken(
                        _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                        claims,
                        expires: DateTime.Now.AddMinutes(1440),
                        signingCredentials: credentials
                    );
                    return new JsonResult(
                       new
                       {
                           message = HttpStatusCode.OK.ToString(),
                           code = HttpStatusCode.OK,
                           response = new JwtSecurityTokenHandler().WriteToken(token)
                       });

            }
            else
            {
                return new JsonResult(
                   new
                   {
                       message = HttpStatusCode.BadRequest.ToString(),
                       code = HttpStatusCode.BadRequest,
                       response = "Este usuario no esta registrado"
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


    [HttpGet]
    [Route("activate")]
    //Endpoint para activar cuenta
    public async Task<ContentResult> PostActivate(Guid token)
    {
        try
        {

            if (context.Usuarios.Any(u=>u.UserTokenActivation == token))
            {
                var usuarioAct = context.Usuarios.Where(u => u.UserTokenActivation == token).First();
                if (usuarioAct.UserExpToken > DateTime.Now)
                {
                    usuarioAct.UserStatus = 1;
                    usuarioAct.UserTokenActivation = null;
                    context.Update(usuarioAct);
                    context.SaveChanges();

                     return new ContentResult
                    {
                        ContentType = "text/html",
                        Content = "<p>usuario activado correctamente.</p> <br> <a href='https://mango-pebble-066da7c0f.2.azurestaticapps.net/login'>Da click aqui para iniciar sesion</a>"
                     }; ; 
                }
                else
                {

                    EnvioActivacion correoA = new EnvioActivacion();

                    Guid tokenAct = Guid.NewGuid();

                    while (context.Usuarios.Any(u => u.UserTokenActivation == tokenAct))
                    {
                        tokenAct = Guid.NewGuid();
                    }

                    string body = "<p>Bienvenido a expendiete medico " + usuarioAct.UserNombre + " para activar tu cuenta porfavor da click en el siguiente enlace:</p><br><a href='"+ apiurl.getapiURL() + "/api/users/activate?token=" + tokenAct.ToString() + "'>Activar cuenta</a>";
                    if (await correoA.enviarCorreo(usuarioAct.UserEmail, "Activacion de cuenta", body, false))
                    {
                        usuarioAct.UserTokenActivation = tokenAct;
                        usuarioAct.UserExpToken = DateTime.Now.AddMinutes(720);

                        context.Update(usuarioAct);
                        context.SaveChanges();


                    }

                    return new ContentResult
                    {
                        ContentType = "text/html",
                        Content = "<p>El link expiro, se le enviara uno nuevo en unos minitos, favor de revisar su correo electronico.</p>"
                    }; ;
                }
            }
            else
            {
                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = "<p>Este link caduco, porfavor contacte al administrador para activar su cuenta</p>"
                }; ;
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
            return new ContentResult
            {
                ContentType = "text/html",
                Content = "<p>"+ jOb .ToString()+ "</p>"
            }; ;

        }
    }

    [HttpPost]
    [Route("sendactivate")]
    //Endpoint para activar cuenta
    public async Task<ActionResult> SendActivate(string token)
    {
        try
        {
            var stream = token;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(stream);
            var mail = jwtSecurityToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;

            DateTime expired = jwtSecurityToken.ValidTo;

            var user = context.Usuarios.Where(u => u.UserEmail == mail).First();

            if ( expired > DateTime.Now)
            {
                EnvioActivacion correoA = new();
                Guid tokenAct = Guid.NewGuid();

                while (context.Usuarios.Any(u => u.UserTokenActivation == tokenAct))
                {
                    tokenAct = Guid.NewGuid();
                }

                user.UserTokenActivation = tokenAct;
                user.UserExpToken = DateTime.Now.AddMinutes(720);

                context.Update(user);
                context.SaveChanges();

                string body = "<p>Bienvenido a expendiete medico " + user.UserNombre + " para activar tu cuenta porfavor da click en el siguiente enlace:</p><br><a href='"+ apiurl.getapiURL() + "/api/users/activate?token=" + tokenAct.ToString() + "'>Activar cuenta</a>";

                if (await correoA.enviarCorreo(user.UserEmail, "Activacion de cuenta", body, false))
                {
                    return new JsonResult(
                   new
                   {
                       message = HttpStatusCode.OK.ToString(),
                       code = HttpStatusCode.OK,
                       response = "se envio un correo con link de activacion al Email "+user.UserEmail.ToString()
                   });
                }
                else {
                    return new JsonResult(
                       new
                       {
                           message = HttpStatusCode.BadRequest.ToString(),
                           code = HttpStatusCode.BadRequest,
                           response = "No se pudo enviar el correo  de activacion al Email " + user.UserEmail.ToString()
                       });
                }
            }
            else
            {

                return new JsonResult(
                       new
                       {
                           message = HttpStatusCode.BadRequest.ToString(),
                           code = HttpStatusCode.BadRequest,
                           response = "El token expiro"
                       });
            }
        }
        catch(Exception ex) {
            var jOb = new
            {
                message = HttpStatusCode.BadRequest.ToString(),
                code = HttpStatusCode.BadRequest,
                response = ex.Message.ToString()
            };

            return BadRequest(jOb);
        }
    }*/
    //Funcion para encriptar aes256 un string
    public static string EncryptString(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        //funcion para desencriptar string con sha256
        public static string DecryptString(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        /*public async Task<ActionResult> Post(string nombre, string password, string primera,
            string sgundoa, string correo, string telefono, string sexo, string edad,
            string domiilio, bool privacidad)
        {
            bool result = Usuario.Equals(s => s.UserEmail == correo && s.UserPass == password);
            if (result)
            {
                return NotFound("Correo existente en la base de datos");
            }
            else if (!privacidad)
            {
                return NotFound("Es obligatorio el consentimiendo de confidencialidad.");
            }
            {

                Startup.Adduser(nombre, password, primera, sgundoa, correo, telefono, sexo, edad, domiilio, privacidad);

                return Ok();
            }
        }*/
    }


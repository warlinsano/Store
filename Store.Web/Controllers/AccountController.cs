using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Web.Data;
using Store.Web.Data.Entities;
using Store.Web.Helpers;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class AccountController : Controller
    {
        //private readonly NorthwindContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;
        private readonly IMailHelper _mailHelper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            //NorthwindContext context,
            IUserHelper userHelper,
            IConfiguration configuration,
            DataContext dataContext,
            IMailHelper mailHelper,
            RoleManager<IdentityRole> roleManager
           )
        {
            //_context = context;
            _roleManager = roleManager;
            _userHelper = userHelper;
            _configuration = configuration;
            _dataContext = dataContext;
            _mailHelper = mailHelper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    //Log user
                    var Resultado = await LogAddAccion(new ActivityLogViewModel
                    {
                        UserEMail = model.Username,
                        Action = "Login",
                        Description = "User logged in",
                        Controller = "Account",
                        Status = true,
                        Type = "AppWeb"
                    });
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "User or password not valid.");
                model.Password = string.Empty;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            //Log user
            var Resultado = await LogAddAccion(new ActivityLogViewModel
            {
                UserEMail = User.Identity.Name,
                Action = "Logout",
                Description = "has closed the section",
                Controller = "Account",
                Status = true,
                Type = "AppWeb"
            });
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMonths(4),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model, IFormFile Image)
        {
            if (!model.termsAccept)
            {
                ModelState.AddModelError(string.Empty, "I must accept the license terms in order to register.");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                if (Image != null && Image.Length > 0)
                {
                    var _iImageHelper = new ImageHelper();
                    model.Photo = _iImageHelper.UploadImageDB(Image);
                    model.PhotoPath = await _iImageHelper.UploadImageDirectoryAsync(Image, "Users");
                }
                var user = await AddUserAsync(model);
                if (user == null)
                {
                    //Para Eliminar la foto fisica..
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Users", model.PhotoPath.Substring(12));
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    return View(model);
                }

                //var owner = new Owner
                //{
                //    Pets = new List<Pet>(),
                //    User = user,
                //};

                //_dataContext.Owners.Add(owner);
                //await _dataContext.SaveChangesAsync();

                var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                var tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                _mailHelper.EnviarMail(model.Username, "Email confirmation",
                    $"<table style = 'max-width: 600px; padding: 10px; margin:0 auto; border-collapse: collapse;'>" +
                    $"  <tr>" +
                    $"    <td style = 'background-color: #34495e; text-align: center; padding: 0'>" +
                    $"       <a href = 'https://www.facebook.com/NuskeCIV/' >" +
                    $"         <img width = '20%' style = 'display:block; margin: 1.5% 3%' src= 'https://veterinarianuske.com/wp-content/uploads/2016/10/line_separator.png'>" +
                    $"       </a>" +
                    $"  </td>" +
                    $"  </tr>" +
                    $"  <tr>" +
                    $"  <td style = 'padding: 0'>" +
                    $"     <img style = 'padding: 0; display: block' src = 'https://veterinarianuske.com/wp-content/uploads/2018/07/logo-nnske-blanck.jpg' width = '100%'>" +
                    $"  </td>" +
                    $"</tr>" +
                    $"<tr>" +
                    $" <td style = 'background-color: #ecf0f1'>" +
                    $"      <div style = 'color: #34495e; margin: 4% 10% 2%; text-align: justify;font-family: sans-serif'>" +
                    $"            <h1 style = 'color: #e67e22; margin: 0 0 7px' > Hola </h1>" +
                    $"                    <p style = 'margin: 2px; font-size: 15px'>" +
                    $"                      El mejor Hospital Veterinario Especializado de la Ciudad de Morelia enfocado a brindar servicios médicos y quirúrgicos<br>" +
                    $"                      aplicando las técnicas más actuales y equipo de vanguardia para diagnósticos precisos y tratamientos oportunos..<br>" +
                    $"                      Entre los servicios tenemos:</p>" +
                    $"      <ul style = 'font-size: 15px;  margin: 10px 0'>" +
                    $"        <li> Urgencias.</li>" +
                    $"        <li> Medicina Interna.</li>" +
                    $"        <li> Imagenologia.</li>" +
                    $"        <li> Pruebas de laboratorio y gabinete.</li>" +
                    $"        <li> Estetica canina.</li>" +
                    $"      </ul>" +
                    $"  <div style = 'width: 100%;margin:20px 0; display: inline-block;text-align: center'>" +
                    $"    <img style = 'padding: 0; width: 200px; margin: 5px' src = 'https://veterinarianuske.com/wp-content/uploads/2018/07/tarjetas.png'>" +
                    $"  </div>" +
                    $"  <div style = 'width: 100%; text-align: center'>" +
                    $"    <h2 style = 'color: #e67e22; margin: 0 0 7px' >Email Confirmation </h2>" +
                    $"    To allow the user,plase click in this link:</ br ></ br > " +
                    $"    <a style ='text-decoration: none; border-radius: 5px; padding: 11px 23px; color: white; background-color: #3498db' href = \"{tokenLink}\">Confirm Email</a>" +
                    $"    <p style = 'color: #b3b3b3; font-size: 12px; text-align: center;margin: 30px 0 0' > Nuskë Clinica Integral Veterinaria 2019 </p>" +
                    $"  </div>" +
                    $" </td >" +
                    $"</tr>" +
                    $"</table>");
                ViewBag.Message = "The instructions to allow your user has been sent to email.";
                return View(model);
            }

            return View(model);
        }

        private async Task<User> AddUserAsync(AddUserViewModel model)
        {
            var user = new User
            {
                Address = model.Address,
                Document = model.Document,
                Email = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username,
                Photo = model.Photo,
                PhotoPath = model.PhotoPath
            };

            var result = await _userHelper.AddUserAsync(user, model.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            var newUser = await _userHelper.GetUserByEmailAsync(model.Username);
            await _userHelper.AddUserToRoleAsync(newUser, "Customer");
            return newUser;
        }

        //public async Task<IActionResult> ChangeUser()
        //{
        //    var owner = await _dataContext.Owners
        //        .Include(o => o.User)
        //        .FirstOrDefaultAsync(o => o.User.UserName.ToLower() == User.Identity.Name.ToLower());
        //    if (owner == null)
        //    {
        //        return NotFound();
        //    }

        //    var view = new EditUserViewModel
        //    {
        //        Address = owner.User.Address,
        //        Document = owner.User.Document,
        //        FirstName = owner.User.FirstName,
        //        Id = owner.Id,
        //        LastName = owner.User.LastName,
        //        PhoneNumber = owner.User.PhoneNumber
        //    };

        //    return View(view);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        //{
        //    if (model is null)
        //    {
        //        throw new ArgumentNullException(nameof(model));
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var owner = await _dataContext.Owners
        //            .Include(o => o.User)
        //            .FirstOrDefaultAsync(o => o.Id == model.Id);

        //        owner.User.Document = model.Document;
        //        owner.User.FirstName = model.FirstName;
        //        owner.User.LastName = model.LastName;
        //        owner.User.Address = model.Address;
        //        owner.User.PhoneNumber = model.PhoneNumber;

        //        await _userHelper.UpdateUserAsync(owner.User);
        //        return RedirectToAction("Index", "Home");
        //    }

        //    return View(model);
        //}

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                var link = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);
                _mailHelper.EnviarMail(model.Email, "MyVet Password Reset", $"<h1>Shop Password Reset</h1>" +
                    $"To reset the password click in this link:</br></br>" +
                    $"<a href = \"{link}\">Reset Password</a>");
                ViewBag.Message = "The instructions to recover your password has been sent to email.";
                return View();
            }
            return View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Password reset successful.";
                    return View();
                }

                ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            ViewBag.Message = "User not found.";
            return View(model);
        }

        // GET: Account
        [HttpGet]
        public async Task<IActionResult> ListUser()
        {
            return View(await _dataContext.Users.ToListAsync());
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> EditUserAndRoles(string id)
        {
            // Create Admin Role
            string roleName = "Facturador";
            var si = await _roleManager.RoleExistsAsync(roleName);
            IdentityResult roleResult;

            // Check to see if Role Exists, if not create it
            if (!si)
            {
                roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            //var user =await _userManager.FindByNameAsync("warlinsano@yopmail.com");
            //var rol2 = await  _userManager.AddToRoleAsync(user, "Developer");
            //var rol1 = await _userManager.AddToRoleAsync(user, "Admin");

            if (id == null)
            {
                return NotFound();
            };

            var userSearched = await _dataContext.Users.FindAsync(id);
            if (userSearched == null)
            {
                return NotFound();
            }

            var MisRoles = (
                 from r in _dataContext.Roles
                 join ru in _dataContext.UserRoles on r.Id equals ru.RoleId
                 where ru.UserId == userSearched.Id
                 select r.Name
                ).ToArray();

            //obtengo todos los roles de la tabla  roles
            var AllRole = _dataContext.Roles.ToArray();

            string[,] todosRoles;
            todosRoles = new string[AllRole.Count(), 2];
            for (int i = 0; i < AllRole.Count(); i++)
            {
                if (MisRoles.Contains(AllRole[i].Name.ToString()))
                {
                    todosRoles[i, 0] = AllRole[i].Name;
                    todosRoles[i, 1] = "true";
                }
                else
                {
                    todosRoles[i, 0] = AllRole[i].Name;
                    todosRoles[i, 1] = "false";
                }
            }
            //todosRoles;
            //User usuario = new User
            //{
            //    Id = userSearched.Id,
            //    PhotoPath = userSearched.PhotoPath,
            //    Photo = userSearched.Photo,
            //    UserName = userSearched.UserName,
            //    NormalizedUserName = userSearched.NormalizedUserName,
            //    Email = userSearched.Email,
            //    todosRoles = todosRoles
            //};
            userSearched.todosRoles = todosRoles;

            //var mir = _context.AspNetRoles(x=>x.)
            //return View(aspNetUsers);
            //ViewBag.Estado = new SelectList(lst, "Value", "Text", usuario.Estado);
            LlenarDdlEstado(userSearched.Estado);
            return View(userSearched);
        }

        //// POST: Usuarios/Edit/5
        //[HttpPost]
        //public ActionResult EditUserAndRoles(User usuario, FormCollection fc)
        //{
        //    string[] RolesCheckeadoRecibidosFc = fc.GetValues("role");
        //    var user = _dataContext.Users.Where(x => x.Id == usuario.Id).FirstOrDefault();
        //    if (ModelState.IsValid)
        //    {
        //        //Verifica si ya existe otro usuario con este Numero_Identificacion
        //        //var verificarSiExiste = _dataContext.Users.Where(x => x.Numero_Identificacion == usuario.Numero_Identificacion.Trim() && x.Id != usuario.Id).Count();

        //        //Verifica si ya existe otro usuario con este Email
        //        var verificarSiExiste = _dataContext.Users.Where(x => x.Email == usuario.Email.Trim() && x.Id != usuario.Id).Count();
        //        if (verificarSiExiste != 0)
        //        {
        //            ////usuario.Numero_Identificacion = user.Numero_Identificacion;
        //            //usuario.Nombre = user.Nombre;
        //            LlenarDdlEstado(usuario.Estado);
        //            //ViewBag.Id_Tipo_Identificacion = new SelectList(db.Tipo_Identificacion, "Id_Tipo_Identificacion", "Tipo_Numero_Identificacion", usuario.Id_Tipo_Identificacion);
        //            ViewBag.msj = ("El  correo " + usuario.Email.ToString() + " ya se encuentra asocciado a un usuario ");
        //            usuario.todosRoles = RolesCheckeado(RolesCheckeadoRecibidosFc);
        //            return View(usuario);
        //        }

        //        try
        //        {
        //            var userManager = new UserManager<ApplicationUser>(
        //                  new UserStore<ApplicationUser>(db));

        //            var roleManager = new RoleManager<IdentityRole>
        //               (new RoleStore<IdentityRole>(db));

        //            //-------------------------- aqui ahi que agregar la validaciones de la cedula y los demas # de Identificacion RNC, etc
        //            ApplicationUser usuarios = userManager.FindById(usuario.Id);
        //            {
        //                //usuarios.Id_Tipo_Identificacion = usuario.Id_Tipo_Identificacion;
        //                ////Numero de Identificasion....
        //                //usuarios.Nombre = usuario.Nombre;
        //                usuarios.UserName = usuario.Email;
        //                usuarios.Email = usuario.Email;
        //                usuarios.Estado = usuario.Estado;
        //            };

        //            string[] RolesCheck = fc.GetValues("role");
        //            var oldRole = userManager.GetRoles(usuario.Id).ToList();

        //            if (RolesCheck == null)
        //            {
        //                for (int i = 0; i < oldRole.Count; i++)
        //                {
        //                    //Remover a usuario del rol
        //                    userManager.RemoveFromRoles(usuario.Id, oldRole[i]);
        //                }
        //            }
        //            else
        //            {
        //                //Eliminar los roles que ya no se tienen
        //                for (int i = 0; i < oldRole.Count; i++)
        //                {
        //                    if (!oldRole[i].Contains(RolesCheck.ToString()))
        //                    {
        //                        //Remover a usuario del rol
        //                        userManager.RemoveFromRoles(usuario.Id, oldRole[i]);
        //                    }
        //                }

        //                //Agregar los roles nuevos
        //                for (int i = 0; i < RolesCheck.Length; i++)
        //                {
        //                    if (!RolesCheck[i].Contains(oldRole.ToString()))
        //                    {
        //                        ////Agregar role a usuario
        //                        userManager.AddToRole(usuario.Id, RolesCheck[i].ToString());
        //                    }
        //                }
        //            }
        //            // TODO: Add update logic here
        //            db.Entry(usuarios).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return RedirectToAction("Index");

        //        }
        //        catch (Exception ex)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    usuario.Numero_Identificacion = user.Numero_Identificacion;
        //    usuario.Nombre = user.Nombre;
        //    LlenarDdlEstado(usuario.Estado);
        //    ViewBag.Id_Tipo_Identificacion = new SelectList(db.Tipo_Identificacion, "Id_Tipo_Identificacion", "Tipo_Numero_Identificacion", usuario.Id_Tipo_Identificacion);
        //    ViewBag.msj = "";
        //    usuario.todosRoles = RolesCheckeado(RolesCheckeadoRecibidosFc);
        //    return View(usuario);
        //}


        // POST: Usuarios  Cambiar el estado a true o false

        //return una matris con los todos lo roles y  Check los roles que tiene este usuario activo... 
        //private string[,] RolesCheckeado(string[] MisRoles)
        //{
        //    //add try casch
        //    var roleManager = new RoleManager<IdentityRole>
        //    (new RoleStore<IdentityRole>(db));

        //    //obtengo todos los roles de la tabla  roles
        //    var AllRole = roleManager.Roles.ToList();

        //    string[,] todosRoles;
        //    todosRoles = new string[AllRole.Count(), 2];
        //    for (int i = 0; i < AllRole.Count(); i++)
        //    {
        //        if (MisRoles.Contains(AllRole[i].Name.ToString()))
        //        {
        //            todosRoles[i, 0] = AllRole[i].Name;
        //            todosRoles[i, 1] = "true";
        //        }
        //        else
        //        {
        //            todosRoles[i, 0] = AllRole[i].Name;
        //            todosRoles[i, 1] = "false";
        //        }
        //    }
        //    return todosRoles;
        //}

        //TODO: QUIRAME, BORRAME DE AHI
        [AllowAnonymous]
        public string nombreWebApi(string Numero_Identificacion)
        {
            //if()
            //Numero_Identificacion.Replace("-", "");
            ///validacion de solo numeros..
            var Nombre = "";
            if (Numero_Identificacion == null || Numero_Identificacion.Length != 11)
            {
                return Nombre;
            }
            //Nombre = clienteAPI.ConsultarNombre(Numero_Identificacion);
            return Nombre;
        }

        public JsonResult CambiarEstado(string Id, bool Estado)
        {
            try
            {
                var Usuarios = _dataContext.Users.Where(x => x.Id == Id).FirstOrDefault();
                Usuarios.Estado = Estado;
                // TODO: Add update logic here
                _dataContext.Entry(Usuarios).State = EntityState.Modified;
                _dataContext.SaveChanges();
                return Json(new { data = true });
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        // GET: ActivityLogs
        public IActionResult LogUsers()
        {
            return View();
            //return View(await _context.ActivityLog.ToListAsync());
        }

        // GET: All the Activity Logs
        public async Task<JsonResult> Datos()
        {
            var activityLog = await _dataContext.ActivityLog.Select(a => new ActivityLog
            {
                IdActivityLog = a.IdActivityLog,
                Date = a.Date,
                Ip = a.Ip,
                MacAddress = a.MacAddress,
                NameDevice = a.NameDevice,
                UserId = a.UserId,
                UserEMail = a.UserEMail,
                UserFullName = a.UserFullName,
                Action = a.Action,
                Controller = a.Controller,
                Description = a.Description,
                Status = a.Status,
                Type = a.Type,
                Photo = _dataContext.Users.Where(x => x.Id == a.UserId).Select(u => u.PhotoPath).FirstOrDefault() == null ?
            _dataContext.Users.Where(x => x.Id == a.UserId).Select(u => u.PhotoBase64).FirstOrDefault() :
            _dataContext.Users.Where(x => x.Id == a.UserId).Select(u => u.PhotoPath).FirstOrDefault()
            }).ToListAsync();
            return Json(new { data = activityLog });

            //return await _activityLogHelper.LogData();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<bool> LogAddAccion(ActivityLogViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.UserEMail);

                if (user != null)
                {
                    var Registro = new ActivityLog();
                    Registro.Date = DateTime.Now;
                    Registro.Ip = NetworkHelper.GetIPClient();
                    Registro.MacAddress = NetworkHelper.GetMacAdreessDeviceClient();
                    Registro.NameDevice = NetworkHelper.GetNameDeviceClient();
                    Registro.UserId = user.Id;
                    Registro.UserEMail = user.Email;
                    Registro.UserFullName = user.FullName;
                    Registro.Controller = model.Controller;
                    Registro.Action = model.Action;
                    Registro.Description = model.Description;
                    Registro.Status = model.Status;
                    Registro.Type = model.Type;
                    _dataContext.Add(Registro);
                    await _dataContext.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        private void LlenarDdlEstado(bool Estado)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Text = "Habilitado", Value = "true" });
            lst.Add(new SelectListItem() { Text = "Desabilitado", Value = "false" });
            ViewBag.Estado = new SelectList(lst, "Value", "Text", Estado);
        }

    }
}

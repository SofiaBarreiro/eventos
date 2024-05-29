using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EventosCeremonial.Helpers;
using EventosCeremonial.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using EventosCeremonial.Data.Response;
using EventosCeremonial.Data;

namespace EventosCeremonial.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IDbConnection DbConnection { get; }

        public RegisterModel(
            UserManager<IdentityUser> userManager,

            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
           RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;


        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string[] Errores { get; set; } = null;

        public class InputModel
        {
            [Required(ErrorMessage = "El campo Correo electrónico es obligatorio")]
            [EmailAddress]
            [Display(Name = "Correo electrónico")]
            public string Email { get; set; }

            [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [StringLength(20, ErrorMessage = "El número de pasaporte es incorrecto", MinimumLength = 2)]
            [DataType(DataType.Text)]
            [Display(Name = "Pasaporte")]
            public string Pasaporte { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirme su contraseña")]
            [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
            public string ConfirmPassword { get; set; }


        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }


        

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            LoggerManger logger = new LoggerManger();

            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var userEncontro = await _signInManager.UserManager.FindByEmailAsync(Input.Email);

            if (userEncontro == null)
            {

                if (ModelState.IsValid)
                {
                    if (!Input.Email.Contains("@trabajo.gob.ar"))
                    {
                        var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };



                        var result = await _userManager.CreateAsync(user, Input.Password);



                        if (await _roleManager.RoleExistsAsync("Administrador") == false)
                        {


                            IdentityRole ir = new IdentityRole { Name = "Administrador", NormalizedName = "ADMINISTRADOR", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() };
                            var result2 = await _roleManager.CreateAsync(ir);


                        }
                        if (await _roleManager.RoleExistsAsync("Participante") == false)
                        {


                            IdentityRole ir2 = new IdentityRole { Name = "Participante", NormalizedName = "PARTICIPANTE", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() };
                            var result24 = await _roleManager.CreateAsync(ir2);


                        }
                        //if (await _roleManager.RoleExistsAsync("Soporte") == false)
                        //{


                        //    IdentityRole ir3 = new IdentityRole { Name = "Soporte", NormalizedName = "SOPORTE", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() };
                        //    var result2 = await _roleManager.CreateAsync(ir3);


                        //}

                        if (result.Succeeded)
                        {

                            try
                            {
                               
                                //if (user.Email == "fegutierrez@trabajo.gob.ar")
                                //{
                                //await _userManager.AddToRoleAsync(user, "Soporte");

                                //}
                                //    else {


                                await _userManager.AddToRoleAsync(user, "Administrador");


                                //    }


                                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                                var callbackUrl = Url.Page(
                                    "/Account/ConfirmEmail",
                                    pageHandler: null,
                                    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                                    protocol: Request.Scheme);

                                await _emailSender.SendEmailAsync(Input.Email,
                                     "Confirme su correo", CorreoElectronico.textoConfirmacióCorreoIdentity(HtmlEncoder.Default.Encode(callbackUrl)));

                                // Add new users whose email starts with 'admin' to the Admin role


                                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                                {
                                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, isConfirmed = false, returnUrl = returnUrl });
                             
                                }
                                else
                                {

                                    await _signInManager.SignInAsync(user, isPersistent: false);
                                    return LocalRedirect(returnUrl);
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.LogError("Error en registro:", ex);
                                ModelState.AddModelError("Error", ex.Message);

                            }


                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(error.Description, error.Description);

                        }
                    }
                    else
                    {

                        ModelState.AddModelError("Debe ingresar al sitio a través de la opción Nacionales y luego elegir Active Directory.", "Debe ingresar al sitio a través de la opción Nacionales y luego elegir Active Directory. Seleccione volver.");

                    }
                }

            }
            else {
                if (!Input.Email.Contains("@trabajo.gob.ar"))
                {
                    ModelState.AddModelError("Se encuentra registrado", "El usuario ingresado ya se encuentra registrado");

                }
                else {


                    
                 ModelState.AddModelError("Se encuentra registrado, nacional", "El usuario ingresado ya se encuentra registrado como nacional. Seleccione volver y elija la opción Nacionales");

                    
                }


            }



            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

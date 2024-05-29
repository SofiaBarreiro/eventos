using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using EventosCeremonial.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventosCeremonial.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;


        public bool IdentidadesDiv { get; set; } = true;
        public bool Login { get; set; } = true;
        public bool DivIngreso { get; set; } = true;
        public bool DivBotones { get; set; } = false;


        
        public LoginModel(SignInManager<IdentityUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "El campo Correo electrónico es obligatorio")]
            [EmailAddress(ErrorMessage = "El formato ingresado es incorrecto. Por favor ingrese una casilla de correo")]
            public string Email { get; set; }

            [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Recordar contraseña")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


            StatusMessage = "";
            ReturnUrl = returnUrl;




        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            LoggerManger logger = new LoggerManger();


            try
            {
                if (!Input.Email.Contains("@trabajo.gob.ar"))
                {
                    if (ModelState.IsValid)
                {


                        returnUrl ??= Url.Content("~/BuscarInvitacion");

                        //returnUrl ??= Url.Content("~/");



                        var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);


                        // This doesn't count login failures towards account lockout
                        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                        var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);


                        if (result.Succeeded)
                        {
                            //var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);

                            var roles = await _signInManager.UserManager.GetRolesAsync(user);


                            var claims = new List<Claim>();

                            claims.Add(new Claim(ClaimTypes.Name, Input.Email));

                            foreach (var role in roles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role));
                            }
                            //var claims = new ClaimsIdentity();

                            claims.Add(new Claim(ClaimTypes.NameIdentifier, Input.Email));



                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);



                            var authProperties = new AuthenticationProperties
                            {
                                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120)
                            };


                            await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                            return LocalRedirect(returnUrl);
                        }
                        if (result.IsLockedOut)
                        {
                           

                            return RedirectToPage("./");
                        }
                        else
                        {
                            if (Input.Email == "" || Input.Password == "")
                            {
                                ModelState.AddModelError(string.Empty, "Debe ingresar los datos solicitados");

                            }
                            else
                            {
                                if (user == null)
                                {

                                    ModelState.AddModelError(string.Empty, "No se encontró usuario con ese correo");

                                }
                                else
                                {

                                    ModelState.AddModelError(string.Empty, "Error");

                                    StatusMessage = "la contraseña es incorrecta";
                                    //return Page();

                                }


                            }


                        }


                    }

                }
                else
                {


                    ModelState.AddModelError("Debe ingresar al sitio a través de la opción Nacionales y luego elegir Active Directory.", "Debe ingresar al sitio a través de la opción Nacionales y luego elegir Active Directory. Seleccione volver.");


                }
                return Page();


            }
            catch (Exception e)
            {
                logger.LogError("Error en ingreso", e);



            }

            return Page();

        }
    }
    

}

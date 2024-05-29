using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using EventosCeremonial.IRepository;
using EventosCeremonial.Helpers;
namespace EventosCeremonial.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task OnGetAsync()
        {
            
        }
        public async Task<IActionResult> OnPostAsync()
        {

            if (!Input.Email.Contains("@trabajo.gob.ar"))
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        ModelState.AddModelError("Usuario no se encontro", "El usuario no existe o no confirmó su correo.");

                        return RedirectToPage("./ForgotPasswordConfirmation");
                    }

                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity", code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        Input.Email, "Recuperar contraseña", CorreoElectronico.recuperarContraseñaIdentity(HtmlEncoder.Default.Encode(callbackUrl), ("&Email=" + Input.Email)));



                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
            }
            else
            {

                ModelState.AddModelError("Debe ingresar al sitio a través de la opción Nacionales y luego elegir Active Directory.", "Debe ingresar al sitio a través de la opción Nacionales y luego elegir Active Directory. Seleccione volver.");


            }
            return Page();
        }
    }
}

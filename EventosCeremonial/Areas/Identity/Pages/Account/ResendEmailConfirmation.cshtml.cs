using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EventosCeremonial.Helpers;
using EventosCeremonial.IRepository;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace EventosCeremonial.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ResendEmailConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "El campo Correo electrónico es obligatorio")]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!Input.Email.Contains("@trabajo.gob.ar"))
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                var returnUrl = Url.Content("~/");

                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user != null && user.EmailConfirmed == false)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = userId, code = code },
                        protocol: Request.Scheme);
                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "Confirme su correo", CorreoElectronico.textoConfirmacióCorreoIdentity(HtmlEncoder.Default.Encode(callbackUrl)));

                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, isConfirmed = false, returnUrl = returnUrl });


                }
                else
                {
                    if (user != null)
                    {
                        if (user.EmailConfirmed == true)
                        {

                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, isConfirmed = true, returnUrl = returnUrl });



                        }

                    }
                    else
                    {
                        ModelState.AddModelError("Usuario null", "El usuario ingresado no se encuentra registrado.");


                        return RedirectToPage("RegisterConfirmation", new { email = "", isConfirmed = false, returnUrl = returnUrl });


                    }




                }
            }
            else {

                ModelState.AddModelError("Debe ingresar al sitio a través de la opción Nacionales y luego elegir Active Directory.", "Debe ingresar al sitio a través de la opción Nacionales y luego elegir Active Directory. Seleccione volver.");



            }



            //ModelState.AddModelError(string.Empty, "Le hemos enviado un correo electrónico a su correo, por favor revise su casilla.");
            return Page();
        }
    }
}

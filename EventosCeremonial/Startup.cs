using EventosCeremonial.Areas.Identity;
using EventosCeremonial.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using DGIIT.Authentication.Adfs;
using Microsoft.Extensions.Logging;
using EventosCeremonial.IRepository;
using EventosCeremonial.Helpers;
using Microsoft.AspNetCore.Authentication.WsFederation;
using System.Threading.Tasks;
using System.Security.Claims;
using EventosCeremonial;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace EventosCeremonial
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;

        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment _env { get; }
        private void AddSwagger(IServiceCollection services)
        {
           

            LoggerManger logger = new LoggerManger();

            var groupName = "v1";

            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Eventos Ceremonial.Api {groupName}",
                    Version = groupName,
                    Description = "Api de eventos Ceremonial",
                    Contact = new OpenApiContact
                    {
                        Name = String.Empty,
                        Email = "soportesistemas@trabajo.gob.ar",
                        Url = new Uri(Configuration["URL"]),
                    }
                });


            });
        }
        public void ConfigureServices(IServiceCollection services)
        {
            LoggerManger logger = new LoggerManger();

            try
            {

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

                services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    //CONFIGURACION DE CONTRASEÑA EN IDENTITY
                    options.SignIn.RequireConfirmedAccount = true;//REQUIERE CONFIRMACION DE CORREO
                    options.Password.RequireDigit = false;//NO REQUIERE NROS
                    options.Password.RequiredLength = 6;//TAMAÑO MÍNIMO
                    options.Password.RequireNonAlphanumeric = false;//NO ES OBLIGATORIO USO DE NROS Y LETRAS
                    options.Password.RequireUppercase = false;//NO ES OBLIGATORIO MAYUSCULAS
                    options.Password.RequireLowercase = false;//NO ES OBLIGATORIO MAYUSCULAS

                }

                )
                    .AddRoles<IdentityRole>()//ROLES DE IDENTITY
                    .AddEntityFrameworkStores<ApplicationDbContext>();
                //var secretKey = Configuration["Token:Secret"];

                services.AddDbContext<EventosCeremonialContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
                services.AddAuthentication(sharedOptions =>
                {
                    //    //sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    //    //sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;

                    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;

                }).AddWsFederation(options =>
                     {


                         options.MetadataAddress = Configuration["MetadataAddress"];
                         options.Wtrealm = Configuration["Wtrealm"];
                         options.Events.OnRedirectToIdentityProvider = ctx =>
                         {
                             ctx.ProtocolMessage.Whr = Configuration["Whr"];
                             return Task.FromResult(0);
                         };
                         options.Events.OnTicketReceived = ctx =>
                         {
                             ctx.ReturnUri = Configuration["URL"] + "/BuscarInvitacion";
                             return Task.FromResult(0);

                         };
                     }).AddCookie();


                //services.AddAuthentication(options =>
                //{

                //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                //    options.RequireAuthenticatedSignIn = false;
                //}).AddJwtBearer(jws =>
                //{
                //    jws.RequireHttpsMetadata = false;
                //    jws.SaveToken = true;
                //    jws.TokenValidationParameters = new TokenValidationParameters()
                //    {

                //        ValidateIssuerSigningKey = true,
                //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                //        ValidateIssuer = false,
                //        ValidateAudience = false
                //    };

                //});
                //services.AddAuthorization();
                //services.AddSingleton<IJwtAuthenticationService>(new JWTHelper(secretKey));

            }
            catch (Exception e)
            {
                logger.LogError("error en conexion a DB", e);
            }

            try
            {
                services.AddRazorPages();
                services.AddServerSideBlazor().AddCircuitOptions(o =>
                    {
                        if (_env.IsDevelopment()) //only add details when debugging
                        {
                            o.DetailedErrors = true;

                        }
                    });
                services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
                AddSwagger(services);

                services.AddDatabaseDeveloperPageExceptionFilter();
                services.AddScoped<IFileUpload, FileUpload>();
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback =
                                (message, cert, chain, errors) => true;
                services.AddSingleton(new HttpClient(httpClientHandler)
                {
                    BaseAddress = new Uri(Configuration["URL"])
                });
                services.AddControllersWithViews();
                services.AddSingleton<IloggerManager, LoggerManger>();
                services.AddTransient<IEmailSender, SendMail>();
                services.AddHttpContextAccessor();

                //services.AddControllers().AddJsonOptions(x =>x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
            }
            catch (Exception e)
            {
                logger.LogError("error en injeccion de dependencias", e);
            }
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            LoggerManger logger = new LoggerManger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
               
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            if (env.EnvironmentName == "Development") {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor API V1");
                });
            }

         

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //NO CAMBIAR ORDEN
            app.UseAuthentication();
            app.UseAuthorization();
            //NO CAMBIAR ORDEN



            //app.Use(async (context, next) =>
            //{
            //    ClaimsPrincipal user = context.User;


            //    if (!user.Identities.Any(x => x.IsAuthenticated))
            //    {

            //        await context.ChallengeAsync(WsFederationDefaults.AuthenticationScheme).ConfigureAwait(false);

            //    }

            //    if (next != null)
            //    {
            //        await next().ConfigureAwait(false);
            //    }
            //});
          
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");

                //endpoints.Map("/LogOutAD", async context =>
                //{
                //    var scheme = WsFederationDefaults.AuthenticationScheme;

                //    var properties = new AuthenticationProperties { RedirectUri = "/" };

                //    await context.Response.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                //    var authenticationSchemes = new[] { CookieAuthenticationDefaults.AuthenticationScheme, scheme };

                //    foreach (var authenticationScheme in authenticationSchemes)
                //    {
                //        await context.SignOutAsync(authenticationScheme, properties);
                //    }
                //});

                //endpoints.Map("/SignOut2", async context =>
                //{

                //    var scheme = IdentityConstants.ExternalScheme;

                //    var authenticationSchemes = new[] { CookieAuthenticationDefaults.AuthenticationScheme, scheme };
                //    var properties = new AuthenticationProperties { RedirectUri = "/" };


                //    foreach (var authenticationScheme in authenticationSchemes)
                //    {
                //        await context.SignOutAsync(authenticationScheme, properties);
                //    }


                //});
            });
            //loggerFactory.AddLog4Net("log4net.config"); // << Add this line
        }
    }
}

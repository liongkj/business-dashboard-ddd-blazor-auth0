using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using JomMalaysia.Framework;
using JomMalaysia.Presentation.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace JomMalaysia.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
       
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>
                
                {
                    options.LoginPath = new PathString("/Account/Login");
                    
                    options.Events = new CookieAuthenticationEvents 
                    
                    {
                    
                        OnValidatePrincipal = context =>
                        {
                            var accessToken = context.Properties.Items[".Token.access_token"] ;
                            var handler = new JwtSecurityTokenHandler();

                            if (handler.ReadToken(accessToken) is JwtSecurityToken tokens)
                            {
                                //get the users tokens
                                // AccessTokenResponse response = null;
                                var exp = tokens.Claims.FirstOrDefault(x => x.Type == "exp")?.Value;
                                if (!long.TryParse(exp, out var parsed)) return Task.CompletedTask;
                               
                                var expires = DateTimeOffset.FromUnixTimeSeconds(parsed);
                                var isExpired =  DateTime.UtcNow > expires;
                                Debug.WriteLine("has Expire:"+ isExpired +" "+ expires.ToLocalTime() + "Datetime now: " +
                                                DateTime.Now);
                                    
                                if (isExpired)
                                {
                                    context.RejectPrincipal();
                                    return Task.CompletedTask;
                                   //if need refresh token, add here
                                }

                                return Task.CompletedTask;
                            }
                           context.RejectPrincipal();
                           return Task.CompletedTask;
                    }  
                    };
                    
                    options.AccessDeniedPath = new PathString("/Error/AccessDeniedError");
                }
            ).AddOpenIdConnect(
                "Auth0", options =>
                {
                    // Set the authority to your Auth0 domain
                    options.Authority = $"https://{Configuration["Auth0:Domain"]}";
                    // Configure the Auth0 Client ID and Client Secret
                    options.ClientId = Configuration["Auth0:ClientId"];
                    options.ClientSecret = Configuration["Auth0:ClientSecret"];
                    options.Scope.Add(Configuration["Auth0:Scope"]);
                    
                    // Set response type to code
                    options.ResponseType =OpenIdConnectResponseType.Code;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        RoleClaimType = ClaimTypes.Role,
                        ValidateIssuer = true,
                        ValidateLifetime = true
                    };
                    options.CallbackPath = new PathString("/success");
                    // Configure the Claims Issuer to be Auth0
                    options.ClaimsIssuer = "Auth0";
                    // Saves tokens to the AuthenticationProperties
                    options.SaveTokens = true;
                    options.Events = new OpenIdConnectEvents
                    {
                        
                        
                       OnRedirectToIdentityProvider = context =>
                        {
                            
                            context.ProtocolMessage.SetParameter("audience", Configuration["Auth0:Audience"]);
                            
                            return Task.FromResult(0);
                        },

                       //Logout Event
                        OnRedirectToIdentityProviderForSignOut = context =>
                        {
                            var logoutUri =
                                $"https://{Configuration["Auth0:Domain"]}/v2/logout?client_id={Configuration["Auth0:ClientId"]}";

                            var postLogoutUri = context.Properties.RedirectUri;
                            if (!string.IsNullOrEmpty(postLogoutUri))
                            {
                                if (postLogoutUri.StartsWith("/"))
                                {
                                    // transform to absolute
                                    var request = context.Request;
                                    postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase +
                                                    postLogoutUri;
                                }

                                logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                            }

                            context.Response.Redirect(logoutUri);
                            context.HandleResponse();

                            return Task.CompletedTask;
                        }
                    };
                   
                   
                   
                });
            
            services.AddAuthorization();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddHttpContextAccessor();
            services.AddResponseCaching();
            services.AddSingleton(Configuration);
            services.AddSingleton(new MapperConfiguration(cfg => { cfg.AddProfile<PresentationProfile>(); })
                .CreateMapper());
      

        }
        public void ConfigureContainer(ContainerBuilder builder) {
            builder.RegisterModule(new PresentationModule());
            builder.RegisterModule(new FrameworkModule());
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
           
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy(); 
            app.UseRouting();
            app.UseResponseCaching();

            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl = 
                    new CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(10)
                    };
                context.Response.Headers[HeaderNames.Vary] = 
                    new[] { "Accept-Encoding" };

                await next();
            });

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages()
                    ;
            });
            
    
        }
    }
}
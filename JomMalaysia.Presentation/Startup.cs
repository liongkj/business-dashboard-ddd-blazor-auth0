using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using JomMalaysia.Framework;
using JomMalaysia.Presentation.Scope;
using JomMalaysia.Presentation.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
        public IServiceProvider ConfigureServices(IServiceCollection services)
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
            { options.AccessDeniedPath = "/Error/AccessDeniedError"; }
            ).AddOpenIdConnect(
                "Auth0", options =>
                {
                    // Set the authority to your Auth0 domain
                    options.Authority = $"https://{Configuration["Auth0:Domain"]}";
                    // Configure the Auth0 Client ID and Client Secret
                    options.ClientId = Configuration["Auth0:ClientId"];
                    options.ClientSecret = Configuration["Auth0:ClientSecret"];
                    // Set response type to code
                    options.ResponseType = "code";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/roles",
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                    };
                    // Configure the scope
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    // Set the callback path, so Auth0 will call back to http://localhost:3000/callback
                    // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard
                    options.CallbackPath = new PathString("/success");
                    // Configure the Claims Issuer to be Auth0
                    options.ClaimsIssuer = "Auth0";

                    options.Events = new OpenIdConnectEvents
                    {
                        OnRedirectToIdentityProvider = context =>
                        {
                            context.ProtocolMessage.SetParameter("audience", "https://localhost:44368/");

                            return Task.FromResult(0);
                        }
                    };
                    // Saves tokens to the AuthenticationProperties
                    options.SaveTokens = true;
                    //Logout Event
                    options.Events = new OpenIdConnectEvents
                    {
                        // handle the logout redirection
                        OnRedirectToIdentityProviderForSignOut = (context) =>
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

            services.AddAuthorization(
                options =>
                {
                    options.AddPolicy("read:merchant",
                        policy => policy.Requirements.Add(new HasScopeRequirement("read:merchant",
                            "https://jomn9.auth0.com/")));
                }
            );

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHttpContextAccessor();

            services.AddSingleton(Configuration);
            services.AddSingleton(new MapperConfiguration(cfg => { cfg.AddProfile<PresentationProfile>(); })
                .CreateMapper());


            // Now register our services with Autofac container.
            var builder = new ContainerBuilder();
            builder.RegisterModule(new PresentationModule());
            builder.RegisterModule(new FrameworkModule());
            builder.Populate(services);
            var container = builder.Build();
            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
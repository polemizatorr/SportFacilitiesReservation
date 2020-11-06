using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportObjectsReservationSystem.Data;
using SportObjectsReservationSystem.Models;
using SportObjectsReservationSystem.Services;


namespace SportObjectsReservationSystem
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
            services.AddControllersWithViews();
            
            services.AddSession();
            services.AddMvc();
            services.AddDistributedMemoryCache();

            services.AddScoped<AccountantService>();
            services.AddAuthentication();

            
            
            services.AddDbContext<SportObjectsReservationContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("SportObjectsReservationContext")));

                services.AddDefaultIdentity<User>()
                    .AddEntityFrameworkStores<SportObjectsReservationContext>();
                
                services.Configure<IdentityOptions>(conf =>
                {
                    conf.Password.RequireDigit = false;
                    conf.Password.RequireLowercase = true;
                    conf.Password.RequireUppercase = false;
                    conf.Password.RequireNonAlphanumeric = false;
                
                    conf.Password.RequiredLength = 5;
                    conf.Password.RequiredUniqueChars = 5;

                    conf.SignIn.RequireConfirmedEmail = false;
                    conf.User.RequireUniqueEmail = true;
                });
                services.AddControllers();
                
                services.AddSession(options => {  
                    options.IdleTimeout = TimeSpan.FromMinutes(30);  
                });
                
                
                
                services.ConfigureApplicationCookie(options => {
                    // Cookie settings
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    // If the LoginPath isn't set, ASP.NET Core defaults 
                    // the path to /Account/Login.
                    options.LoginPath = "/accountant/Login";
                    // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                    // the path to /Account/AccessDenied.
                    //options.AccessDeniedPath = "/AccessDenied";
                    options.SlidingExpiration = true;

                });
                
                

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

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseSession();
            app.UseMvc();





            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Accountant}/{action=Login}/{id?}");
            });
        }
    }
}
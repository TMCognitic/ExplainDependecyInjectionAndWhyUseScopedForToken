using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1
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
            services.AddMemoryCache();
            services.AddHttpContextAccessor();

            services.AddSession(opt =>
            {
                opt.Cookie.HttpOnly = true;
                opt.Cookie.IsEssential = true;
                opt.Cookie.Name = "Cookiedoe";
                opt.IdleTimeout = TimeSpan.FromDays(365 * 99);
            });

            services.AddSingleton(sp =>
            {
                ISession session = sp.GetService<IHttpContextAccessor>().HttpContext.Session;
                return new Singleton()
                {
                    Uid = (session.Keys.Contains("Uid")) ? session.GetString("Uid") : null
                };
            });
            services.AddScoped(sp =>
            {
                ISession session = sp.GetService<IHttpContextAccessor>().HttpContext.Session;
                return new Scoped()
                {
                    Uid = (session.Keys.Contains("Uid")) ? session.GetString("Uid") : null
                };
            });
            services.AddTransient(sp =>
            {
                ISession session = sp.GetService<IHttpContextAccessor>().HttpContext.Session;
                return new Transient()
                {
                    Uid = (session.Keys.Contains("Uid")) ? session.GetString("Uid") : null
                };
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
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

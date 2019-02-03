using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iEdonNetPortal.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace iEdonNetPortal
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

            services.AddDbContext<PortalContext>(options => options.UseMySql(Configuration.GetConnectionString("MySQL")));
            services.AddTransient<PortalContext>();
            services.AddDistributedMemoryCache();                    // 在内存使用SESSION
            services.AddSession(options => {
                options.Cookie.Name = "iEdonSid";
                options.IdleTimeout = TimeSpan.FromSeconds(3600);    // 设置session的过期时间(单位秒，与session中expire一致，为1小时)
                options.Cookie.HttpOnly = true;                      // 设置在浏览器不能通过js获得该cookie的值
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
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
            }

            app.UseStaticFiles();
            app.UseSession();

            app.Use((context, next) =>
            {
                if (context.Request.Path != "/" && !context.Request.Path.ToString().StartsWith("/Home/"))
                {
                    string username = PublicMethods.GetLoggedInUsername(context);
                    if (username == null)
                    {
                        context.Response.Redirect("/Home/Index");
                    }
                }
                return next();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

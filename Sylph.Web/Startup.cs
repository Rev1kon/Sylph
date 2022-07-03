using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sylph.Data;
using Sylph.Data.Models;
using Microsoft.Data.SqlClient;
using Sylph.Web.Services;
using Sylph.Web.Services.Interfaces;

namespace Sylph.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IGoodsService, GoodsService>();
            services.AddTransient<Repository<Site>>();

            var builder = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("DefaultConnection"))
            {
                UserID = Configuration["Database:User"],
                Password = Configuration["Database:Password"],
                DataSource = Configuration["Database:DataSource"],
                InitialCatalog = Configuration["Database:DB"]
            };

            services.AddDbContext<DataContext>(
                options => options.UseSqlServer(
                    builder.ConnectionString,
                    x => x.MigrationsAssembly("Sylph.Data")
                )
            );

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

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

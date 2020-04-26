using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodRecall_Group11.DataAccess;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FoodRecall_Group11.Models;
using FoodRecall_Group11.ModelDto;

namespace FoodRecall_Group11
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
            services.AddMvc();
            services.AddAutoMapper(typeof(Startup));

            // Use SQL Database if in Azure, otherwise, use SQLite
           // if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["DbConnection:ConnectionString"]));
           // else
            //    services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:FoodRecall:ConnectionString"]));

            services.AddCors(
                options => options.AddPolicy("AllowCors",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                    })
            );
          //  services.AddMvc();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //This makes sure that the database and tables are created as per the Models.
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
            }

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

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Results, FoodRecallDto>();
            });

            IMapper mapper = config.CreateMapper();

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

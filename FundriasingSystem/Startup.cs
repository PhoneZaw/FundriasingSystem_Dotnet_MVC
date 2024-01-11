using FundraisingApp.Repositories;
using FundraisingApp.Services;
using FundriasingSystem.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundriasingSystem
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

            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("BetterTogetherDb"));
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    //options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    //options.SlidingExpiration = true;
                    //options.AccessDeniedPath = "/Forbidden";
                });

            //services.AddAuthentication("Admin")
            //    .AddCookie("Admin", options =>
            //    {
            //        //options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            //        //options.SlidingExpiration = true;
            //        //options.AccessDeniedPath = "/Forbidden";
            //        options.LoginPath = "/Admin/Login";
            //    });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<StaffRoleService>();
            services.AddScoped<StaffService>();
            services.AddScoped<DonorService>();
            services.AddScoped<CampaignService>();
            services.AddScoped<CampaignTypeService>();
            services.AddScoped<DonationService>();
            services.AddScoped<PaymentMethodService>();
            services.AddScoped<ExpenseTypeService>();
            services.AddScoped<ExpenseService>();
            services.AddScoped<CertificateService>();
            services.AddScoped<SuggestionService>();

            services.AddHttpContextAccessor();

            services.AddScoped<HttpContextAccessor>();
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

            app.UseAuthentication();

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

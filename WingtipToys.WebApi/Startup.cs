using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WingtipToys.DataAccessLayer;
using AutoMapper;
using WingtipToys.BusinessLogicLayer.Services;
using WingtipToys.BusinessLogicLayer;

namespace WingtipToys.WebApi
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
            services.AddDbContext<WingtipContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddAutoMapper(this.GetType().Assembly);
            services.AddAutoMapper(typeof(WingtipProfile));
            /*var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new WingtipProfile());
            });*/

            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddControllers();
            services.AddSwaggerGen();

            // cross origin 
            services.AddCors(options =>
            {
                options.AddPolicy("FrontEndClient"
                    , builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("FrontEndClient");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WingtipToys Rest API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.EF;

namespace Bihua.WebApi
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //注入跨域策略
            services.AddCors(config => config.AddPolicy("AllowAcors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials()));

            //注入MogoDbContext,连接字符串在appsettings.json ->ConnectionStrings:MongoDb
            //完整mongodb文件下载请参照：https://github.com/haisa86/bihua.db
            services.AddScoped(factory =>
            {
                return new MongoDBContext(Configuration.GetConnectionString("MongoDb"));
            });
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
                app.UseHsts();
            }

            //设置跨域策略，允许api可以跨域访问
            app.UseCors("AllowAcors");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

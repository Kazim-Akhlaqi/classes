﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Movies.Apis;
using Movies.Services;
using System;

namespace Movies
{
    public class Startup 
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRazorPages();
            services.AddGrpc();
            services.AddServerSideBlazor();
            services.AddSingleton<IGreeter, Greeter>();
        }

        public void Configure(
            IApplicationBuilder app, 
            ILogger<Startup> logger, 
            IWebHostEnvironment environment,
            IGreeter greeter)
        {
            app.UseDeveloperExceptionPage();

            //if (environment.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/error");
            //}
            
            app.UseStatusCodePages();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(e =>
            {
                e.MapControllers();
                e.MapRazorPages();
                e.MapBlazorHub();
                e.MapGrpcService<DirectorService>();

                e.MapGet("/error", ctx =>
                {
                    throw new Exception("Ouch!");
                });

                e.MapGet("/speak", async ctx =>
                {
                    logger.LogInformation("I wil speak...");
                    await ctx.Response.WriteAsync(greeter.GetGreeting());
                });
            });

            
        }

       
    }
}

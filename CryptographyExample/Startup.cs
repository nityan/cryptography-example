/*
 * Copyright 2016-2018 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: nitya
 * Date: 2018-11-4
 */

using System;
using System.Threading.Tasks;
using CryptographyExample.Data;
using CryptographyExample.Models;
using CryptographyExample.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CryptographyExample
{
	/// <summary>
	/// Represents startup for the application.
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// The logger factory.
		/// </summary>
		private readonly ILoggerFactory loggerFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="Startup" /> class.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <param name="loggerFactory">The logger factory.</param>
		public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
		{
			this.Configuration = configuration;
			this.loggerFactory = loggerFactory;
		}

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <value>The configuration.</value>
		public IConfiguration Configuration { get; }

		/// <summary>
		/// Configures the application.
		/// </summary>
		/// <param name="app">The application.</param>
		/// <param name="env">The env.</param>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			var logger = this.loggerFactory.CreateLogger<Startup>();

			if (env.IsDevelopment())
			{
				//app.UseBrowserLink();
				//app.UseDeveloperExceptionPage();
				//app.UseDatabaseErrorPage();
			}
			else
			{
				// force all the unhandled errors to show the not found page
				// so that we mask as much as possible from the user as to what happened
				// however, our not found action will log everything that happened for later use
				app.UseExceptionHandler("/Error/NotFound");

				// security strict transport header
				// indicates to the browser and client side application
				// to force all content over HTTPS when loading resources
				// such as images, css, js, and other content
				app.UseHsts();
			}

			// adds a custom request processor to the request pipeline
			// the reason for this is to add custom functionality to our request pipeline
			app.Use(async (context, next) =>
			{
				try
				{
					// calls the next function in the pipeline
					await next.Invoke();
				}
				catch (Exception e)
				{
					// log any exception which occurred during the request processing
					logger.LogError(env.IsDevelopment() ? $"Unexpected error: {e}" : $"Unexpected error: {e.Message}");
				}
			});

			// add a custom middleware, which does the following
			// before each response rewrite various headers to add security to the response
			app.Use(async (context, next) =>
			{
				// when the response writing begins
				context.Response.OnStarting((state) =>
				{
					// deny the X-Frame-Options
					// indicates to the browser or another application, that this content should not be allowed to be
					// included in a frame
					context.Response.Headers["X-Frame-Options"] = "deny; allow-sites=https://example.com";

					// prevent "sniffing" of the content type
					// this prevents applications from guessing the content type of files or responses
					context.Response.Headers["X-Content-Type-Options"] = "nosniff";

					// block cross site scripting
					context.Response.Headers["X-XSS-Protection"] = "1; mode=block";

					// force the browser to not cache anything
					context.Response.Headers["Cache-Control"] = "no-cache";

					return Task.CompletedTask;
				}, context);

				await next.Invoke();
			});

			app.UseStaticFiles(new StaticFileOptions
			{
				OnPrepareResponse = context =>
				{
					context.Context.Response.Headers["X-Frame-Options"] = "deny";
					context.Context.Response.Headers["X-Content-Type-Options"] = "nosniff";
					context.Context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
					context.Context.Response.Headers["Cache-Control"] = "no-cache";
				}
			});

			app.UseAuthentication();

			// TODO
			app.UseHttpsRedirection();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("CryptographyConnection")));

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddLogging(c =>
			{
				c.AddConfiguration(this.Configuration.GetSection("Logging"));
				c.AddEventSourceLogger();
			});

			// Add application services
			services.AddTransient<ICertificateService, CertificateService>();
			services.AddTransient<ICryptoService, CryptoService>();
			services.AddTransient<ICreditCardService, CreditCardService>();
			services.AddTransient<IEmailSender, EmailSender>();
			services.AddTransient<ILoggerService, LoggerService>();

			services.AddMvc();
		}
	}
}
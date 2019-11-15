using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrderApi.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;

namespace OrderApi
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

            services.AddScoped<OrderContext>();

            services.AddCors(c =>
            {
                c.AddDefaultPolicy(d =>                
                    d.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
                
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Order API",
                    Description = "Order api for foodkart",
                    Version = "1.0",
                    Contact = new Contact
                    {
                        Name = "Chandan Kumar",
                        Email = "chandanK@hexaware.com",
                        Url = "https://github.com/chandan169/"
                    }
                });
            });

            services.AddAuthentication(c =>
            {
                c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            ).AddJwtBearer(c =>
            {
                c.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer=true,
                    ValidateLifetime=true,
                    ValidateIssuerSigningKey=true,
                    ValidIssuer=Configuration.GetValue<string>("Jwt:issuer"),
                    ValidAudience=Configuration.GetValue<string>("Jwt:audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Jwt:secret")))
                };
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            if (env.IsDevelopment()) { }
            app.UseSwaggerUI(config=>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Odrer API");
                config.RoutePrefix = "";
            });

            app.UseCors();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}

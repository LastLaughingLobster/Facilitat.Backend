using Facilitat.CLOUD.Models.Settings;
using Facilitat.CLOUD.Repositories.Schedule;
using Facilitat.CLOUD.Repositories.Towers;
using Facilitat.CLOUD.Repositories.Users;
using Facilitat.CLOUD.Services.Schedule;
using Facilitat.CLOUD.Services.Towers;
using Facilitat.CLOUD.Services.Users;
using Facilitat.CLOUD.Utils.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;
using System.Data;
using System.Text;

namespace Facilitat.CLOUD
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

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IScheduleService, ScheduleOrderService>();

            services.AddScoped<ITowerRepository, TowerRepository>();
            services.AddScoped<ITowerService, TowerService>();

            services.AddScoped<IDbConnection>(sp => new SqlConnection(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false; // Note: This should be true in production.
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = Configuration["Jwt:Issuer"], // Correct way to access the Issuer
                            ValidAudience = Configuration["Jwt:Audience"], // Correct way to access the Audience
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])), // Correct way to access the SecretKey
                            ClockSkew = TimeSpan.Zero
                        };
                    });



            var rabbitMQConfig = new MyRabbitMQSettings();
            Configuration.Bind("RabbitMQ", rabbitMQConfig);
            services.AddSingleton(rabbitMQConfig); // Add MyRabbitMQSettings to the DI container

            var factory = new ConnectionFactory()
            {
                HostName = rabbitMQConfig.Hostname,
                UserName = rabbitMQConfig.Username,
                Password = rabbitMQConfig.Password
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            // Register the channel as a singleton
            services.AddSingleton<IModel>(channel);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Facilitat CLOUD",
                    Description = "Making your life easier",
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://127.0.0.1:5501")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS, etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseCors("AllowSpecificOrigin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

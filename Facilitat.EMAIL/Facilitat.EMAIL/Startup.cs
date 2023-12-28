using Facilitat.EMAIL.Services;
using Facilitat.EMAIL.Models.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Data;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Facilitat.EMAIL.Repositories.Schedule;
using Facilitat.EMAIL.Services.Schedule;

namespace Facilitat.EMAIL
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

            // Bind and register the RabbitMQ settings
            var rabbitMQConfig = new MyRabbitMQSettings();
            Configuration.Bind("RabbitMQ", rabbitMQConfig);
            services.AddSingleton(rabbitMQConfig);

            // RabbitMQ setup
            var factory = new ConnectionFactory()
            {
                HostName = rabbitMQConfig.Hostname,
                UserName = rabbitMQConfig.Username,
                Password = rabbitMQConfig.Password
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            services.AddSingleton<IModel>(channel);

            // AWS SES setup
            var awsOptions = Configuration.GetAWSOptions();
            awsOptions.Credentials = new BasicAWSCredentials(
                Configuration["AWS:AccessKey"],
                Configuration["AWS:SecretKey"]
            );
            awsOptions.Region = Amazon.RegionEndpoint.GetBySystemName(Configuration["AWS:Region"]);
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonSimpleEmailService>();

            // Database connection setup
            services.AddScoped<IDbConnection>(sp => new SqlConnection(Configuration.GetConnectionString("DefaultConnection")));

            // Repository and Service registration
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IScheduleService, ScheduleOrderService>();

            services.AddHostedService(provider => new ScheduleOrderConsumer(
                provider.GetRequiredService<IModel>(),
                rabbitMQConfig.QueueName,
                provider.GetRequiredService<IServiceScopeFactory>()));


            // CORS setup
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://127.0.0.1:5501")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowSpecificOrigin");
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
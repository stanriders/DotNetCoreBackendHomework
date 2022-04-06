
using Dapper;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.BusinessLogic.RequestHandlers;
using WebApi.Engine;
using WebApi.Mappings;
using WebApi.Queue;
using WebApi.Storage;
using WebApi.Storage.Contracts;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);

            services.AddAutoMapper(config =>
            {
                config.AddProfile(typeof(MappingProfile));
            });

            // Обработчики запросов
            services.AddScoped<AddTodoItemRequestHandler>();
            services.AddScoped<UpdateTodoItemRequestHandler>();
            services.AddScoped<GetAllTodoItemsRequestHandler>();
            services.AddScoped<GetTodoItemRequestHandler>();

            // Хранилища
            services.AddScoped<ITodoItemRepository, TodoItemRepository>();
            services.AddTransient<IDbConnectionAdapter, NpgsqlConnectionAdapter>();

            var rabbitMqConfig = Configuration.GetSection("RabbitMq");

            services.AddMassTransit(x =>
            {
                x.AddConsumer<UpdateTodoItemMessageConsumer>().Endpoint(e =>
                {
                    e.Name = Configuration["QueueName"];
                });

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqConfig["Url"], configurator =>
                    {
                        configurator.Username(rabbitMqConfig["Username"]);
                        configurator.Password(rabbitMqConfig["Password"]);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });
            services.AddMassTransitHostedService(true);

            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:5001";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                    };
                });

            services.AddControllers(options =>
            {
                options.Filters.Add<ErrorFilter>();
            });

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" }); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
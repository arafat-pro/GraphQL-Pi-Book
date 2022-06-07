using System;
using System.Text;
using GraphQL.Server.Ui.Voyager;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Pi_Books.Data;
using Pi_Books.Data.Models;
using Pi_Books.Data.Services;
using Pi_Books.Exceptions;
using Pi_Books.GraphQL;
using Pi_Books.GraphQL.Mutations;
using Pi_Books.GraphQL.Queries;
using Pi_Books.GraphQL.Types;

namespace Pi_Books
{
    public class Startup
    {
        public string ConnectionString { get; set; }
        public string HealthCheckConnectionString { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("DefaultConnectionString");
            HealthCheckConnectionString = Configuration.GetConnectionString("HealthCheckConnectionString");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddOData(option=>option
                .EnableQueryFeatures() //For All Functionality in case of being the API a private/internal.
                .AddRouteComponents("odata", GetEdmModel())
                //.Filter()
                //.OrderBy()
                //.Expand()
                );

            //Configure DbContext with SQL
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString));

            //GraphQL
            services.AddPooledDbContextFactory<AppDbGraphQLContext>(options => options.UseSqlServer(ConnectionString));
            services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .AddType<PublisherType>()
                .AddType<BookType>()
                .AddFiltering()
                .AddSorting()
                .AddInMemorySubscriptions();
            //.AddProjections();

            //Configure the Services
            services.AddTransient<BooksService>();
            services.AddTransient<AuthorsService>();
            services.AddTransient<PublishersService>();
            services.AddTransient<LogsService>();

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;

                //config.ApiVersionReader = new HeaderApiVersionReader("custom-version-header");
                //config.ApiVersionReader = new MediaTypeApiVersionReader();
            });

            //Token Validation Parameters
            var tokenValidationparemeters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JWT:Secret"])),

                ValidateIssuer = true,
                ValidIssuer = Configuration["JWT:Issuer"],

                ValidateAudience = true,
                ValidAudience = Configuration["JWT:Audience"],

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(tokenValidationparemeters);

            //Add Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            //Add Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //Add JWT Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = tokenValidationparemeters;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pi_Books", Version = "v1" });
            });

            //Health Checks
            var appBuilder = WebApplication.CreateBuilder();
            services.AddHealthChecks()
                .AddSqlServer(appBuilder.Configuration.GetConnectionString("HealthCheckConnectionString"));
            services.AddHealthChecksUI().AddInMemoryStorage();
        }

        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Publisher>("PublishersOData");
            return builder.GetEdmModel();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder appBuilder, IWebHostEnvironment environment, ILoggerFactory loggerFactory)
        {
            if (environment.IsDevelopment())
            {
                appBuilder.UseDeveloperExceptionPage();
                appBuilder.UseSwagger();
                appBuilder.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pi_Books v1"));

                //Health Checks
                //original code for .net v 6 (where there is only program file and no startup file)
                //appBuilder.MapHealthChecks("/healthcheck");
                appBuilder.UseHealthChecks("/healthcheck", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                appBuilder.UseHealthChecksUI();
            }

            appBuilder.UseWebSockets();

            appBuilder.UseHttpsRedirection();

            appBuilder.UseRouting();

            //Authentication & Authorization
            appBuilder.UseAuthentication();
            appBuilder.UseAuthorization();

            //Exception Handling by Middle-ware
            //appBuilder.ConfigureBuiltInExceptionHandler();
            appBuilder.ConfigureBuiltInExceptionHandler(loggerFactory);

            //Exception Handling by Custom Exception Handler Middle-ware
            //appBuilder.ConfigureCustomExceptionHandler();

            appBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGraphQL();
                //Below was appeared in the comment section of the source video but was not been required for my case
                //endpoints.MapGraphQLVoyager("/graphql-voyager");
            });

            appBuilder.UseGraphQLVoyager(new VoyagerOptions()
            {
                GraphQLEndPoint = "/graphql",
            }, "/graphql-voyager");

            AppDbInitializer.InitialDbSeed(appBuilder);

            AppDbInitializer.SeedRoles(appBuilder).Wait();
        }
    }
}
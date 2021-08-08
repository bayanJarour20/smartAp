using Elkood.Web.Domain.ConfigureServices;
using Elkood.Web.Infrastructure.Configuration;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartStart.Model.Security;
using SmartStart.SqlServer.DataBase;
using SmartStart.SqlServer.DataBase.Seed;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VueCliMiddleware;

namespace SmartStart
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddElDbContext<SmartStartDbContext>(
            (options) =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<SmartStartDbContext>().AddDefaultTokenProviders();
            
            services.AddElRepositoryInject("SmartStart.Repository.Main", 
                                           "SmartStart.Repository.General",
                                           "SmartStart.Repository.Setting",
                                           "SmartStart.Repository.Security",
                                           "SmartStart.Repository.Shared");


            services.AddHttpClient("fcm", c => c.BaseAddress = new Uri("https://fcm.googleapis.com"));

            services.AddSpaStaticFiles(configuration: options => { options.RootPath = "clientApp"; });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });
            services.AddDataProtection();

            services.AddStackExchangeRedisCache(options => options.Configuration = this.Configuration["Redis:ConnectionString"]);


            #region -   jwt   -

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
             .AddJwtBearer(options =>
             {
                 options.SaveToken = true;

                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = Configuration["Jwt:Issuer"],
                     ValidAudience = Configuration["Jwt:Issuer"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                 };
             });

            #endregion
            #region -   Swagger   -

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Smart Start", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {{
                       new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = "Bearer"
                             }
                         },
                         new string[] {}
                    }});
            });

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocExpansion(DocExpansion.None);
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Start v1");
            });

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();
            app.UseSpaStaticFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapToVueCliProxy(
                                   "{*path}",
                                   new SpaOptions { SourcePath = "clientApp" },
                                   npmScript: (System.Diagnostics.Debugger.IsAttached) ? "serve" : null,
                                   regex: "Compiled successfully",
                                   forceKill: true,
                           wsl: false // Set to true if you are using WSL on windows. For other operating systems it will be ignored
                                   );
            });

            //app.UseSqlServerSeed<SmartStartDbContext>(async (context, provider) => {
            //    await context.Database.MigrateAsync();
            //    await context.Database.EnsureCreatedAsync();
            //    //await SecuritySeed.InitializeAsync(provider);
            //    await DataSeed.InitializeAsync(provider);
            //});

        }
    }
}

using AnimeBrowser.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace AnimeBrowser_API
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
            services.AddDbContext<AnimeBrowserContext>(options => options.UseNpgsql(Configuration.GetConnectionString("AnimeBrowser")).EnableSensitiveDataLogging());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AnimeBrowser_API", Version = "v1" });
            });

            services.AddAuthentication("Bearer")
              .AddJwtBearer("Bearer", options =>
              {
                  options.Authority = Configuration.GetValue<string>("IdentityServerSettings:AuthorityUrl", "https://localhost:44310");  //IS4 url-e
                  options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                  {
                      ValidAudiences = Configuration.GetValue<IEnumerable<string>>("IdentityServerSettings:ValidAudiences", new List<string> { "AnimeBrowser_API", "AnimeBrowser_API_Admin" }),
                      NameClaimType = Configuration.GetValue<string>("IdentityServerSettings:TokenValidationClaimName", "name"),
                      RoleClaimType = Configuration.GetValue<string>("IdentityServerSettings:TokenValidationClaimRole", "role")
                  };

                  options.SaveToken = true;
              });

            services.AddAuthorization(options =>
            {
                //Read
                options.AddPolicy("AnimeInfoRead", policy => policy.RequireClaim("scope", "anime_info-read"));
                options.AddPolicy("EpisodeRead", policy => policy.RequireClaim("scope", "episode-read"));
                options.AddPolicy("GenreRead", policy => policy.RequireClaim("scope", "genre-read"));
                options.AddPolicy("RatingRead", policy => policy.RequireClaim("scope", "rating-read"));
                options.AddPolicy("SeasonRead", policy => policy.RequireClaim("scope", "season-read"));
                options.AddPolicy("UserListRead", policy => policy.RequireClaim("scope", "user_list-read"));
                //Write
                options.AddPolicy("UserListWrite", policy => policy.RequireAuthenticatedUser().RequireClaim("scope", "user_list-write"));
                options.AddPolicy("RatingWrite", policy => policy.RequireAuthenticatedUser().RequireClaim("scope", "rating-write"));
                //Admin
                options.AddPolicy("AnimeInfoAdmin", policy => policy.RequireAuthenticatedUser().RequireClaim("scope", "anime_info-admin"));
                options.AddPolicy("EpisodeAdmin", policy => policy.RequireAuthenticatedUser().RequireClaim("scope", "episode-admin"));
                options.AddPolicy("GenreAdmin", policy => policy.RequireAuthenticatedUser().RequireClaim("scope", "genre-admin"));
                options.AddPolicy("RatingAdmin", policy => policy.RequireAuthenticatedUser().RequireClaim("scope", "rating-admin"));
                options.AddPolicy("SeasonAdmin", policy => policy.RequireAuthenticatedUser().RequireClaim("scope", "season-admin"));
                options.AddPolicy("UserListAdmin", policy => policy.RequireAuthenticatedUser().RequireClaim("scope", "user_list-admin"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AnimeBrowser_API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

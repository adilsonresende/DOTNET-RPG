using Microsoft.AspNetCore.Authentication.JwtBearer;
using DOTNET_RPG.Services.CharacterSkillService;
using Microsoft.Extensions.DependencyInjection;
using DOTNET_RPG.Services.CharacterService;
using Microsoft.Extensions.Configuration;
using DOTNET_RPG.Services.WeaponService;
using DOTNET_RPG.Services.FightService;
using DOTNET_RPG.Services.AuthService;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using DOTNET_RPG.Data;
using System.Text;

namespace DOTNET_RPG
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x => x.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IWeaponService, WeaponService>();
            services.AddScoped<ICharacterSkillService, CharacterSkillService>();
            services.AddScoped<IFightService, FightService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings").GetSection("Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

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

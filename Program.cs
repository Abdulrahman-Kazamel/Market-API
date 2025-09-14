using System.Text;
using Api.Data;
using Api.Filters;
using Api.Helpers;
using Api.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // //register my Db Context to the DI container as sccoped bt Default
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure JWT
            var jwtOptions = builder.Configuration.GetSection("JWT").Get<JWTOptoionsHelper>();
            builder.Services.AddSingleton(jwtOptions);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigninKey))
                    };
                });

            // Register framework services
            builder.Services.AddControllers(options =>
                    options.Filters.Add<PermissionBasedAuthorizationFilter>()

                );
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure middleware pipeline
            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<LoggingMiddleware>();
            app.UseMiddleware<TimmingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}


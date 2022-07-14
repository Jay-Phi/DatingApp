using System;
using API.Interfaces;
using API.Services;
using API.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace API.Extensions
{
	public static class ApplicationServiceExtensions
	{
        //this : déclaration de la donnée à hériter (extended value) 
		public static IServiceCollection AddApplicationServices (this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddDbContext<DataContext>(options =>
            {
                //Ci-dessous, Default connection fait référence aux données situées dans appsetings.json
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }

		
	}
}

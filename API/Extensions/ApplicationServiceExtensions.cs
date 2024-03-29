﻿using System;
using AutoMapper;
using API.Interfaces;
using API.Services;
using API.Data;
using API.Helpers;
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
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddDbContext<DataContext>(options =>
            {
                //Ci-dessous, Default connection fait référence aux données situées dans appsetings.json
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }

		
	}
}

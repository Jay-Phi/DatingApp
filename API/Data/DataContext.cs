using System;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using API.Entities;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
namespace API.Data
{

	public class DataContext : DbContext 
	{
		public DbSet<AppUser> Users { get; set; }

		public DataContext(DbContextOptions  options ) : base (options)
        {

        }
	}

}


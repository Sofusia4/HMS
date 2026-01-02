using HMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data
{
	public class ApplicationContext : IdentityDbContext<User>
	{
        public DbSet<UserDelete> UserDeletes { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
			: base(options)
		{
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

	}
}

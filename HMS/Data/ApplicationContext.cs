using HMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data
{
	public class ApplicationContext : IdentityDbContext<User>
	{
        public DbSet<UserDelete> UserDeletes { get; set; }
		public DbSet<Hotel> Hotels { get; set; }
		public DbSet<Room> Rooms { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
			: base(options)
		{
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Hotel>()
				   .HasMany<Room>(s => s.Rooms)
				   .WithOne(c => c.Hotel);

			//modelBuilder.Entity<Publication>().Property(e => e.TotalViews).HasDefaultValue(1);
			//modelBuilder.Entity<Publication>().Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
			//Добавить метод с установкой картинки по умолчанию для записи

			base.OnModelCreating(modelBuilder);
		}


	}
}

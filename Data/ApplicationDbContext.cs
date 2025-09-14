using Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions options ) : base(options)
        {
            
        }


        public DbSet<Product> products { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<UserPermission> userPermissions { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Product>().ToTable("Products");
        //    modelBuilder.Entity<User>().ToTable("Users");
        //    modelBuilder.Entity<UserPermission>().ToTable("UserPermission")
        //        .HasKey(x => new { x.UserId , x.permision });

        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserPermission>().ToTable("UserPermission")
                .HasKey(x => new { x.UserId, x.permision });

        }

    }
}

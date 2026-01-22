using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Models;



namespace MyProject.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<ClientProfile> ClientProfiles { get; set; }
    public DbSet<AdminProfile> AdminProfiles { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Reservation> Reservations { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ClientProfile>()
       .HasOne(p => p.User)
       .WithOne()
       .HasForeignKey<ClientProfile>(p => p.UserId)
       .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Table>()
            .HasIndex(t => new { t.RestaurantId, t.TableNumber })
            .IsUnique();

        builder.Entity<Reservation>()
        .HasOne(r => r.User)
        .WithMany() 
        .HasForeignKey(r => r.UserId)
        .OnDelete(DeleteBehavior.Restrict); 

        builder.Entity<Reservation>()
            .HasOne(r => r.ClientProfile)
            .WithMany() 
            .HasForeignKey(r => r.ClientProfileId)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.Entity<Reservation>()
            .HasOne(r => r.Restaurant)
            .WithMany() 
            .HasForeignKey(r => r.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.Entity<Reservation>()
            .HasOne(r => r.Table)
            .WithMany()
            .HasForeignKey(r => r.TableId)
            .OnDelete(DeleteBehavior.Restrict);

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}

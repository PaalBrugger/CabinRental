using Microsoft.EntityFrameworkCore;
using CabinRental.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class ApplicationDBContext : IdentityDbContext <IdentityUser>
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
    }

    public DbSet<Cabin> Cabins { get; set; }
    public DbSet<CabinImage> CabinImages { get; set; }
    public DbSet<Reservation> Reservations { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Cabin)
            .WithMany(c => c.Reservations)
            .HasForeignKey(r => r.CabinId);
        
        modelBuilder.Entity<Cabin>().HasData(
            new Cabin
            {
                Id = 1, Name = "Lake Cabin", Address = "Cabin Street 1", City = "Bergen", Price = 100, Description = "Surrounded by snow-draped pines, this cozy retreat offers a crackling fireplace, stunning views, and pure serenity. 🌨️🔥"
            },
            new Cabin
            {
                Id = 2, Name = "Mountain Cabin", Address = "Cabin Street 2", City = "Bergen", Price = 200, Description ="Experience the magic of winter in this rustic log cabin. Snuggle up with a warm drink and watch the snow fall outside. ☕❄️" 
            },
            new Cabin
            {
                Id = 3, Name = "River Cabin", Address = "Cabin Street 3", City = "Bergen", Price = 300, Description = "Tucked away in a snowy forest, this charming cabin is the perfect retreat for a peaceful and cozy getaway. 🌲🏡"
            },
            new Cabin
            {
                Id = 4, Name = "Forrest Cabin", Address = "Cabin Street 4", City = "Bergen", Price = 400, Description = "Escape to a winter wonderland in this cozy, snow-covered cabin. Warm up by the fireplace and enjoy breathtaking mountain views. ❄️🔥"
            });

        modelBuilder.Entity<CabinImage>().HasData(
            new CabinImage { Id = 1, CabinId = 1, ImagePath = "/Images/Cabin/Cabin1.webp" },
            new CabinImage { Id = 2, CabinId = 1, ImagePath = "/Images/Cabin/Cabin1Interior.webp" },
            new CabinImage { Id = 3, CabinId = 2, ImagePath = "/Images/Cabin/Cabin2.webp" },
            new CabinImage { Id = 4, CabinId = 2, ImagePath = "/Images/Cabin/Cabin2Interior.webp" },
            new CabinImage { Id = 5, CabinId = 3, ImagePath = "/Images/Cabin/Cabin3.webp" },
            new CabinImage { Id = 6, CabinId = 3, ImagePath = "/Images/Cabin/Cabin3Interior.webp" },
            new CabinImage { Id = 7, CabinId = 4, ImagePath = "/Images/Cabin/Cabin4.webp" },
            new CabinImage { Id = 8, CabinId = 4, ImagePath = "/Images/Cabin/Cabin4Interior.webp" });
    }
}
using Microsoft.EntityFrameworkCore;
using AdvertApi.EntityFrameworkContext;

namespace AdvertApi.Entities
{
    public class AdvertisingDbContext : DbContext
    {
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Client> Clients { get; set; }

        public AdvertisingDbContext()
        {

        }

        public AdvertisingDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Building>(entity =>
            {
                entity.HasKey(e => e.IdBuilding);
                entity.Property(e => e.IdBuilding).ValueGeneratedOnAdd();
                entity.Property(e => e.Street).IsRequired();
                entity.Property(e => e.Height).HasColumnType("decimal(6,2)");
                entity.Ignore(c => c.Campaigns);

                entity.ToTable("Building");

            });

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.HasKey(e => e.IdCampaign);
                entity.Property(e => e.IdCampaign).ValueGeneratedOnAdd();
                entity.Property(e => e.PricePerSquareMeter).HasColumnType("decimal(6,2)");
                entity.Property(e => e.Building1).IsRequired();
                entity.Property(e => e.Building2).IsRequired();

                entity.ToTable("Campaign");

                entity.HasMany(c => c.Banners)
                       .WithOne(c => c.Campaign)
                       .HasForeignKey(c => c.IdCampaign)
                       .OnDelete(DeleteBehavior.Cascade);
                      // .IsRequired();

                entity.Ignore(c => c.Building1);
                entity.Ignore(c => c.Building2);

                entity.HasOne(c => c.Building1)
                     .WithMany(c => c.FromIdBuildingCampaigns)
                     .OnDelete(DeleteBehavior.Restrict)
                     .HasForeignKey(c => c.FromIdBuilding);

                entity.HasOne(c => c.Building2)
                    .WithMany(c => c.ToIdBuildingCampaigns)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasForeignKey(c => c.ToIdBuilding);

            });

            

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.IdClient);
                entity.Property(e => e.IdClient).ValueGeneratedOnAdd();
                entity.Property(e => e.Login).IsRequired();
                entity.ToTable("Client");

                entity.HasMany(e => e.Campaigns)
                       .WithOne(e => e.Client)
                       .HasForeignKey(e => e.IdClient)
                       .IsRequired();

            });

            modelBuilder.Entity<Banner>(entity =>
            {
                entity.HasKey(e => e.IdAdvertisement);
                entity.Property(e => e.IdAdvertisement).ValueGeneratedOnAdd();
                entity.Property(e => e.Price).HasColumnType("decimal(6,2)");
                entity.Property(e => e.Area).HasColumnType("decimal(6,2)");
                entity.Property(e => e.Name).IsRequired();
                entity.ToTable("Banner");

            });


            modelBuilder.Seed();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    public class CardDbContext : DbContext
    {
        public CardDbContext() : base("")
        {
            Database.SetInitializer<CardDbContext>(new CardDbInitializer());
        }

        public DbSet<CharacterCard> CharacterCards { get; set; }
        public DbSet<DistrictCard> DistrictCards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<CharacterCard>().HasKey(c => c.Id);

            modelBuilder.Entity<CharacterCard>().Property(p => p.Name)
                .IsRequired()
                .HasColumnOrder(2);

            modelBuilder.Entity<CharacterCard>().Map(m =>
            {
                m.Properties(p => new {p.Id, p.Name, p.CoordinateX, p.CoordinateY, p.TextureSourceFileName});
                m.ToTable("CharacterCardInfo");
            });


            modelBuilder.Entity<DistrictCard>().HasKey(d => d.Id);

            modelBuilder.Entity<DistrictCard>().Property(p => p.Name)
                .IsRequired()
                .HasColumnOrder(2);

            modelBuilder.Entity<DistrictCard>().Property(p => p.Cost)
                .IsRequired()
                .HasColumnOrder(3);

            modelBuilder.Entity<DistrictCard>().Property(p => p.Color)
                .IsRequired()
                .HasColumnOrder(4);

            modelBuilder.Entity<DistrictCard>().Map(m =>
            {
                m.Properties(p => new {p.Id, p.Name, p.Cost, p.Color, p.CoordinateX, p.CoordinateY, p.TextureSourceFileName});
                m.ToTable("DistrictCardInfo");
            });
        }
    }
}

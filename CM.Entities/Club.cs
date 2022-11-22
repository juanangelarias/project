using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class Club: BaseEntity, IBaseEntity
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public long? ClubTypeId { get; set; }
    public long? CountryId { get; set; }
    public string? District { get; set; }

    public ClubType? ClubType { get; set; }
    public Country? Country { get; set; }
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Club>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.Code)
                .HasMaxLength(20);

            e.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            e.Property(p => p.District)
                .HasMaxLength(6);

            e.HasIndex(i => i.Name);

            e.HasOne(o => o.Country)
                .WithMany()
                .HasForeignKey(k => k.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(o => o.ClubType)
                .WithMany()
                .HasForeignKey(k => k.ClubTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
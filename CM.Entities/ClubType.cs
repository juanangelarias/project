using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class ClubType: BaseEntity, IBaseEntity
{
    public string? Name { get; set; }
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<ClubType>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(20);

            e.HasIndex(i => i.Name)
                .IsUnique();
        });
    }
}
using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class Country: BaseEntity, IBaseEntity
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Country>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.Code)
                .HasMaxLength(3)
                .IsRequired();

            e.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            e.HasIndex(i => i.Code);

            e.HasIndex(i => i.Name);
        });
    }
}
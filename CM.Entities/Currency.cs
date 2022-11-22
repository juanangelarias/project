using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class Currency: BaseEntity, IBaseEntity
{
    public string? Name { get; set; }
    public string? Symbol { get; set; }
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Currency>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(20);

            e.Property(p => p.Symbol)
                .IsRequired()
                .HasMaxLength(3);

            e.HasIndex(i => i.Name)
                .IsUnique();
        });
    }
}